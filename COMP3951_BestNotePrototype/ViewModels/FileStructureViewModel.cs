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
    /// Exception raised when an unsupported system object is encountered.
    /// </summary>
    class UnsupportedSystemObjectException : Exception
    {
        public UnsupportedSystemObjectException(string Message) : base(Message) { }
    }

    /// <summary>
    /// Handles logic for creating ITreeView items from file system objects.
    /// 
    /// A proxy for the services provided by the FileManagerService class.
    /// </summary>
    class FileStructureViewUtils
    {
        delegate ITreeViewItem CreateTreeItemBase(FileSystemInfo FileSystemInfo, int Level, Thickness? IndentationPadding);
        delegate IBNFolder CreateFolderItemBase(DirectoryInfo DirectoryInfo);
        delegate IBNFile CreateFileItemBase(FileInfo FileInfo);

        //static readonly Dictionary<Type, CreateTreeItemBase> FileSystemTypeMap  = new()
        //{
        //    { typeof(FileInfo), (FileSystemInfo, Level, IndentationPadding) =>  CreateTreeViewFile((FileInfo)FileSystemInfo, Level, IndentationPadding)},
        //    { typeof(DirectoryInfo), (FileSystemInfo, Level, IndentationPadding) => CreateTreeViewFolder((DirectoryInfo)FileSystemInfo, Level, IndentationPadding) }
        //};

        //static readonly Dictionary<Type, CreateFileItemBase> FileItemTypeMap = new()
        //{

        //};

        /// <summary>
        /// Maps extension to concrete implementaiton.
        /// </summary>
        static readonly Dictionary<string, Type> FileExtensionTypeMap = new()
        {
            { ".md", typeof(MarkdownFile) }
        };

        /// <summary>
        /// Returns an observable collection of ITreeViewItems.
        /// Returns an empty singleton list if there are no children.
        /// A hard reload of the items inside of the specified directory,
        /// root directory if `TargetDirectory` is null.
        /// </summary>
        /// <param name="TargetCollection"></param>
        /// <param name="FileManagerService"></param>
        /// <param name="TargetDirectory"></param>
        /// <param name="Level"></param>
        /// <param name="IndentationPadding"></param>
        /// <returns></returns>
        public static void LoadFileSystemObjects(
            FileManagerService FileManagerService,
            AlertService AlertManagerService,
            ObservableCollection<BestFileTreeItemViewModel> TargetCollection,
            DirectoryInfo? TargetDirectory = null,
            int Level = 0,
            Thickness? IndentationPadding = null
        ) {
            try
            {
                TargetCollection.Clear();

                var contents = FileManagerService.GetDirectoryInfoContents(TargetDirectory?.FullName);

                foreach (var item in contents)
                {
                    ITreeViewItem TreeItem = CreateTreeViewItem(FileManagerService, item, Level, IndentationPadding);
                    BestFileTreeItemViewModel TreeItemViewModel = new(TreeItem, FileManagerService, AlertManagerService);
                    TargetCollection.Add(TreeItemViewModel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"The operation could not be completeed {ex}");
            }
        }

        /// <summary>
        /// Loads the contents of a 
        /// </summary>
        /// <param name="FileManagerService"></param>
        /// <param name="TargetFolder"></param>
        public static void LoadFileSystemObjects(
            FileManagerService FileManagerService,
            AlertService AlertManagerService,
            BestFileTreeItemViewModel TargetItem
        )
        {
            if (TargetItem.TreeViewItem is FolderTreeItem FolderItem)
            {
                LoadFileSystemObjects(
                    FileManagerService,
                    AlertManagerService,
                    FolderItem.Children,
                    FolderItem.DirectoryInfo,
                    FolderItem.ItemLevel,
                    FolderItem.IndentationPadding
                );
            }
        }

        /// <summary>
        /// Creates a new ITreeViewItem item w/ concrete implementation determined by extension.
        /// </summary>
        /// <param name="FileManagerService"></param>
        /// <param name="FileInfo"></param>
        /// <param name="ItemLevel"></param>
        /// <param name="IndentationPadding"></param>
        /// <returns></returns>
        private static ITreeViewItem CreateTreeViewFile(
            FileManagerService FileManagerService,
            FileInfo FileInfo,
            int ItemLevel,
            Thickness IndentationPadding
        ) {
            // Map to extension

            var FileExtension = FileInfo.Extension.ToLower();

            Debug.WriteLine($"File system info object {FileInfo} w/ extension {FileExtension}");

            var MarkdownFile = new MarkdownFile(FileInfo, FileManagerService);
            var FileTreeItem = new FileTreeItem(ItemLevel, IndentationPadding, MarkdownFile);

            return FileTreeItem;
        }

        /// <summary>
        /// Creates a new ITreeViewItem w/ concrete implementaion determined by the extension.
        /// </summary>
        /// <param name="FileManagerService"></param>
        /// <param name="DirectoryInfo"></param>
        /// <param name="ItemLevel"></param>
        /// <param name="IndentationPadding"></param>
        /// <returns></returns>
        private static ITreeViewItem CreateTreeViewFolder(
            FileManagerService FileManagerService,
            DirectoryInfo DirectoryInfo,
            int ItemLevel,
            Thickness IndentationPadding
        ) {
            // Default is windows folder

            Debug.WriteLine($"File system info object {DirectoryInfo}.");

            var WindowsFolder = new WindowsFolder(DirectoryInfo, FileManagerService);
            var FolderTreeItem = new FolderTreeItem(ItemLevel, IndentationPadding, WindowsFolder);

            return FolderTreeItem;
        }

        /// <summary>
        /// Uses maps of file system types to tree view item implementations to instantiate tree view items.
        /// </summary>
        /// <param name="FileSystemInfo"></param>
        /// <returns></returns>
        public static ITreeViewItem CreateTreeViewItem(
            FileManagerService FileManagerService,
            FileSystemInfo FileSystemInfo,
            int ItemLevel = 0,
            Thickness? IndentationPadding = null
        ) {
            int ChildLevel = (ItemLevel == 0) ? 0 : ItemLevel + 10;
            Thickness ChildIndentationPadding = IndentationPadding + new Thickness(10, 0, 0, 0) ?? Thickness.Zero;

            if (FileSystemInfo is FileInfo FileInfo)
            {
                return CreateTreeViewFile(FileManagerService, FileInfo, ChildLevel, ChildIndentationPadding);
            }
            else if (FileSystemInfo is DirectoryInfo DirectoryInfo)
            {
                return CreateTreeViewFolder(FileManagerService, DirectoryInfo, ChildLevel, ChildIndentationPadding);
            }
            else
            {
                throw new UnsupportedSystemObjectException($"Unsupported object type {FileSystemInfo}");
            }
        }


    }

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
