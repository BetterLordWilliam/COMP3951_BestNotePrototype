using CommunityToolkit.Mvvm.Input;
using BestNote_3951.Models.FileSystem;
using BestNote_3951.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig.Extensions.SelfPipeline;

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
        public ITreeViewItem        TreeViewItem { get; private set; }
        public FileManagerService   FileManagerService { get; private set; }
        public AlertService         AlertService { get; private set; }

        [ObservableProperty]
        public partial bool Expanded { get; set; } = false;

        [ObservableProperty]
        public partial string RenameItemText { get; set; } = "";

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
        /// Creates a note file as a child of this item.
        /// This command should only be available if ITreeViewItem is capable of having children.
        /// </summary>
        [RelayCommand]
        public void CreateChildFile(string NewFileName)
        {
            try
            {
                if (!TreeViewItem.CanHaveChildren)
                    throw new NotImplementedException();

                // Use the FileManagerService to create a child file
                FileManagerService.CreateFile(NewFileName, ((IBNFolder)TreeViewItem).DirectoryInfo.FullName);
            }
            catch (NotImplementedException ex)
            {

            }
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
                if (TreeViewItem is FileTreeItem FileTreeItem)
                {
                    FileInfo UpdatedFileInfo = FileManagerService.RenameFile(NewItemName, FileTreeItem.FileInfo);
                    FileTreeItem.FileInfo = UpdatedFileInfo;
                    FileTreeItem.ItemName = UpdatedFileInfo.Name;
                }
                if (TreeViewItem is FolderTreeItem FolderTreeItem)
                {
                    DirectoryInfo UpdatedDirectoryInfo = FileManagerService.RenameFolder(NewItemName, FolderTreeItem.DirectoryInfo);
                    FolderTreeItem.DirectoryInfo = UpdatedDirectoryInfo;
                    FolderTreeItem.ItemName = UpdatedDirectoryInfo.Name;
                }

                Debug.WriteLine($"Worked\n\nUpdated name: {TreeViewItem.ItemName}");
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine($"Error renaming the file tree item {ex}");
                AlertService.ShowAlertAsync("Invalid Action", "You changes could not be applied.");
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
    }
}
