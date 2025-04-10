using CommunityToolkit.Mvvm.Input;
using BestNote_3951.Models.FileSystem;
using BestNote_3951.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig.Extensions.SelfPipeline;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;

///
/// Will Otterbein
/// March 23 2025
/// 
namespace BestNote_3951.ViewModels
{

    /// <summary>
    /// Tree view model object.
    /// </summary>
    public partial class BestFileTreeItemViewModel : ObservableObject
    {
        public ITreeViewItem                        TreeViewItem { get; private set; }
        public FileManagerService                   FileManagerService { get; private set; }
        public AlertService                         AlertService { get; private set; }

        [ObservableProperty]
        public partial bool                         Expanded { get; set; } = false;

        [ObservableProperty]
        public partial string                       RenameItemText { get; set; } = "";


        /// <summary>
        /// Constructor for the BestFileTreeViewModel.
        /// </summary>
        /// <param name="TreeViewItem"></param>
        /// <param name="FileManagerService"></param>
        /// <param name="AlertService"></param>
        public BestFileTreeItemViewModel(
            ITreeViewItem       TreeViewItem,
            FileManagerService  FileManagerService,
            AlertService        AlertService
        ) {
            this.TreeViewItem       = TreeViewItem;
            this.FileManagerService = FileManagerService;
            this.AlertService       = AlertService;
        }

        /// <summary>
        /// Rename the tree view item.
        /// </summary>
        /// <param name="NewItemName"></param>
        [RelayCommand]
        public void RenameItem(string NewItemName)
        {
            Debug.WriteLine($"Rename item command invoked, new name: {NewItemName}");

            if (string.IsNullOrWhiteSpace(NewItemName))
            {
                AlertService.ShowAlertAsync("Invalid Action", "Item name cannot be empty.");
                return;
            }

            try
            {
                TreeViewItem.Rename(NewItemName);
                Debug.WriteLine($"Worked\n\nUpdated name: {TreeViewItem.ItemName}");
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine($"Error renaming the file tree item {ex}");
                AlertService.ShowAlertAsync("Invalid Action", "You changes could not be applied.");
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"Error renaming the file tree item {ex}");
                AlertService.ShowAlertAsync("Invalid Action", $"{ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        /// <summary>
        /// Retrieves the contents of a specified Parent tree item.
        /// </summary>
        /// <param name="Parent"></param>
        [RelayCommand]
        public void RetrieveContents()
        {
            Debug.WriteLine($"Retrieve file called. {TreeViewItem is FolderTreeItem}");

            if (!Expanded && TreeViewItem is FolderTreeItem)
            {
                FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, AlertService, this);
                Expanded = true;
            }
            else if (Expanded)
            {
                Expanded = false;
            }
        }

        /// <summary>
        /// Adds a file to a specified parent item in the tree view.
        /// </summary>
        /// <param name="Parent"></param>
        [RelayCommand]
        public void AddFile()
        {
            Debug.WriteLine("Add File method called");

            try
            {
                if (TreeViewItem is FolderTreeItem FolderTreeItem)
                {
                    FileInfo NewFileInfo = FileManagerService.CreateFile(TargetPath: FolderTreeItem.DirectoryInfo.FullName);
                    ITreeViewItem NewItem = FileStructureViewUtils.CreateTreeViewItem(FileManagerService, NewFileInfo, FolderTreeItem.ItemLevel, FolderTreeItem.IndentationPadding);
                    NewItem.Parent = FolderTreeItem;
                    BestFileTreeItemViewModel NewFileViewModel = new BestFileTreeItemViewModel(NewItem, FileManagerService, AlertService);
                    FolderTreeItem.Children.Add(NewFileViewModel);
                }
            }
            catch (Exception ex)
            {
                AlertService.ShowAlertAsync("Invalid action", "The action you have take is one the system cannot accomodate.");
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Adds a folder to a specified parent item in the tree view.
        /// </summary>
        /// <param name="Parent"></param>
        [RelayCommand]
        public void AddFolder(FolderTreeItem Parent)
        {
            Debug.WriteLine("Add folder method called.");

            try
            {
                if (TreeViewItem is FolderTreeItem FolderTreeItem)
                {
                    DirectoryInfo NewFileInfo = FileManagerService.CreateDirectory(TargetPath: FolderTreeItem.DirectoryInfo.FullName);
                    ITreeViewItem NewItem = FileStructureViewUtils.CreateTreeViewItem(FileManagerService, NewFileInfo, FolderTreeItem.ItemLevel, FolderTreeItem.IndentationPadding);
                    NewItem.Parent = FolderTreeItem;
                    BestFileTreeItemViewModel NewFileViewModel = new BestFileTreeItemViewModel(NewItem, FileManagerService, AlertService);
                    FolderTreeItem.Children.Add(NewFileViewModel);
                }
            }

            catch (Exception ex)
            {
                AlertService.ShowAlertAsync("Invalid action", "The action you have take is one the system cannot accomodate.");
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Deletes the file tree item from the tree and file system.
        /// </summary>
        [RelayCommand]
        public void Delete()
        {
            try
            {
                // Recursive file delete
                if (TreeViewItem.HasChildren && TreeViewItem is FolderTreeItem FolderItem)
                {
                    AlertService.ShowConfirmation("Confirm", "Would you like to recursively delete children?", (result =>
                    {
                        if (result)
                        {
                            FolderItem.DeleteAll();
                            TreeViewItem?.Parent?.Children.Remove(this);
                        }
                    }));
                }
                else
                {
                    TreeViewItem.Delete();
                    TreeViewItem?.Parent?.Children.Remove(this);
                }
            }
            catch (System.IO.IOException ex)
            {

                Debug.WriteLine($"Exception {ex}");
            }
            catch (Exception ex)
            {
                AlertService.ShowAlertAsync("Invalid action", "The action you have taken is one the system cannot accomodate.");
                Debug.WriteLine($"Exception {ex}");
            }
        }

        /// <summary>
        /// Handles the on drop command.
        /// </summary>
        [RelayCommand]
        public void Drop(BestFileTreeItemViewModel? Dragged)
        {
            try
            {
                if (TreeViewItem is FolderTreeItem ParentFolder && Dragged is not null)
                {
                    // Move the item in the system
                    Dragged.TreeViewItem.Move(ParentFolder);

                    // Update the state object children contents
                    ParentFolder.AddChild(Dragged);
                    Dragged.TreeViewItem.Parent?.RemoveChild(Dragged);
                    Dragged.TreeViewItem.Parent = ParentFolder;

                    Debug.WriteLine($"Destination Item: {TreeViewItem.ItemName}\tNew item: {Dragged.TreeViewItem.ItemName}");
                }
            }
            catch (Exception ex)
            {
                AlertService.ShowAlertAsync("Action Could Not Be Complete", "Could not move item to the desired location.");
                Debug.WriteLine($"Exception in the file system {ex}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        [RelayCommand]
        public void OpenFile()
        {
            // If the item is indeed a file
            if (TreeViewItem is null || TreeViewItem is not FileTreeItem)
                return;

            Debug.WriteLine($"Test Trigger on double-click.");

            try
            {
                // IBNFile sourceFile = (TreeViewItem as FileTreeItem).source

                // Send message that a file is being opened, attach the IBNFile object.
                WeakReferenceMessenger.Default.Send<FileOpenedMessage>(new FileOpenedMessage((FileTreeItem)TreeViewItem));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Something has gone terribly wrong!");
            }
        }
    }
}
