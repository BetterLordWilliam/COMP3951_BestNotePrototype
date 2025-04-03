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
        public partial BestFileTreeItemViewModel? Dragger { get; set; } = null;

        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>
        [ObservableProperty]
        public partial BestFileTreeItemViewModel Root { get; private set; }

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

            // Root folder
            var RootFolder = new WindowsFolder(FileManagerService.BestNoteDirectory, FileManagerService);
            var RootFolderTreeItem = new FolderTreeItem(0, Thickness.Zero, RootFolder);

            Root = new BestFileTreeItemViewModel(RootFolderTreeItem, FileManagerService, AlertService);

            FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, AlertService, Root);
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
    }
}
