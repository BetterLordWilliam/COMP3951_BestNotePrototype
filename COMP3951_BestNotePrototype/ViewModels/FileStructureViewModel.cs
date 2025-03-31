using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using BestNote_3951.Models.FileSystem;
using BestNote_3951.Services;
using Syncfusion.Pdf;
using System.IO;
using System.Net.Http.Headers;
using System.ComponentModel;
using System.Reflection.Metadata;

///
/// Will Otterbein
/// March 12 2025
/// 
namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// Handles the logic for the file structure view.
    /// 
    /// Observable object just simplifies a buttload of boilerplater code so we can use the observer pattern out of the box easily.
    /// </summary>
    public partial class FileStructureViewModel : ObservableObject
    {
        private readonly FileManagerService FileManagerService;
        private readonly AlertService AlertService;

        [ObservableProperty]
        public partial string TestingInputName { get; set; }

        [ObservableProperty]
        public partial BestFileTreeItemViewModel? Dragger { get; set; } = null;

        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>
        public ObservableCollection<BestFileTreeItemViewModel> FileSystem { get; private set; } = new ObservableCollection<BestFileTreeItemViewModel>();

        /// <summary>
        /// Initializes the file structure view model with.
        /// </summary>
        /// <param name="AlertService"></param>
        /// <param name="FileManagerService"></param>
        public FileStructureViewModel(
            AlertService AlertService,
            FileManagerService FileManagerService
        ) {
            // FileSystem = GenerateSource();
            this.FileManagerService = FileManagerService;
            this.AlertService = AlertService;
            TestingInputName = "Will Testing Item";

            FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, AlertService, FileSystem);
        }

        [RelayCommand]
        public void OpenFile()
        {
            // TODO: open file logic
        }

        /// <summary>
        /// Sets the value of the dragged item
        /// </summary>
        /// <param name="Dragged"></param>
        [RelayCommand]
        public void Drag(BestFileTreeItemViewModel Dragged)
        {
            Debug.WriteLine($"Dragged item: {Dragged.TreeViewItem.ItemName}");
            Dragger = Dragged;
        }

        /// <summary>
        /// Returns the dragger reference to null.
        /// </summary>
        [RelayCommand]
        public void EndDrag()
        {
            Dragger = null;
        }

        /// <summary>
        /// TEMP, show an alert message.
        /// </summary>
        /// <param name="AlertMessage"></param>
        [RelayCommand]
        public void ShowAlertMessage(string AlertMessage)
        {
            AlertService.ShowAlertAsync("Info", AlertMessage);
        }

        /// <summary>
        /// Retrieves the contents of a specified Parent tree item.
        /// </summary>
        /// <param name="Parent"></param>
        [RelayCommand]
        public void RetrieveContents(BestFileTreeItemViewModel? ParentModel = null)
        {
            Debug.WriteLine($"Retrieve file called. {ParentModel is null}");

            if (ParentModel is null)
            {
                FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, AlertService, FileSystem);
                return;
            }
            else if (ParentModel.TreeViewItem is FolderTreeItem FolderTreeItem)
            {
                FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, AlertService, FolderTreeItem.Children);
            }
        }

        /// <summary>
        /// Adds a file to a specified parent item in the tree view.
        /// </summary>
        /// <param name="Parent"></param>
        [RelayCommand]
        public void AddFile(FolderTreeItem Parent)
        {
            Debug.WriteLine("Add File method called");

            try
            {
                FileInfo NewFileInfo = FileManagerService.CreateFile(TargetPath: Parent.DirectoryInfo.FullName);
                ITreeViewItem NewItem = FileStructureViewUtils.CreateTreeViewItem(FileManagerService, NewFileInfo, Parent.ItemLevel, Parent.IndentationPadding);
                BestFileTreeItemViewModel NewViewModel = new (NewItem, FileManagerService, AlertService);
                Parent.Children.Add(NewViewModel);
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
                DirectoryInfo NewDirectoryInfo = FileManagerService.CreateDirectory(TargetPath: Parent.DirectoryInfo.FullName);
                ITreeViewItem NewItem = FileStructureViewUtils.CreateTreeViewItem(FileManagerService, NewDirectoryInfo, Parent.ItemLevel, Parent.IndentationPadding);
                BestFileTreeItemViewModel NewViewModel = new (NewItem, FileManagerService, AlertService);
                Parent.Children.Add(NewViewModel);
            }
            catch (Exception ex)
            {
                AlertService.ShowAlertAsync("Invalid action", "The action you have take is one the system cannot accomodate.");
                Debug.WriteLine(ex);
            }
        }
    }
}
