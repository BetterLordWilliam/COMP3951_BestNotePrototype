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
            FileManagerService                  FileManagerService,
            ObservableCollection<ITreeViewItem> TargetCollection,
            DirectoryInfo?                      TargetDirectory = null,
            int                                 Level = 0,
            Thickness?                          IndentationPadding = null
        ) {
            try
            {
                TargetCollection.Clear();

                var contents = FileManagerService.GetDirectoryInfoContents(TargetDirectory?.FullName);
                int ChildLevel = (Level == 0) ? 0 : Level + 10;
                Thickness ChildIndentationPadding = IndentationPadding + new Thickness(10, 0, 0, 0) ?? Thickness.Zero;

                foreach (var item in contents)
                {
                    TargetCollection.Add(CreateTreeViewItem(FileManagerService, item, ChildLevel, ChildIndentationPadding));
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
            FileManagerService      FileManagerService,
            FolderTreeItem          TargetFolder
        )
        {
            LoadFileSystemObjects(
                FileManagerService,
                TargetFolder.Children,
                TargetFolder.DirectoryInfo,
                TargetFolder.ItemLevel,
                TargetFolder.IndentationPadding
            );
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
            FileManagerService  FileManagerService,
            FileInfo            FileInfo,
            int                 ItemLevel,
            Thickness           IndentationPadding
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
            FileManagerService  FileManagerService,
            DirectoryInfo       DirectoryInfo,
            int                 ItemLevel,
            Thickness           IndentationPadding
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
            FileManagerService  FileManagerService,
            FileSystemInfo      FileSystemInfo,
            int                 ItemLevel = 0,
            Thickness?          IndentationPadding = null
        ) {
            Thickness TargetIndentationPadding = IndentationPadding ?? Thickness.Zero;

            if (FileSystemInfo is FileInfo FileInfo)
            {
                return CreateTreeViewFile(FileManagerService, FileInfo, ItemLevel, TargetIndentationPadding);
            }
            else if (FileSystemInfo is DirectoryInfo DirectoryInfo)
            {
                return CreateTreeViewFolder(FileManagerService, DirectoryInfo, ItemLevel, TargetIndentationPadding);
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
        public ObservableCollection<ITreeViewItem> FileSystem { get; private set;  } = new ObservableCollection<ITreeViewItem>();

        /// <summary>
        /// Initializes the file structure view model with.
        /// </summary>
        /// <param name="AlertService"></param>
        /// <param name="FileManagerService"></param>
        public FileStructureViewModel(
            AlertService        AlertService,
            FileManagerService  FileManagerService
        ) {
            // FileSystem = GenerateSource();
            this.FileManagerService = FileManagerService;
            this.AlertService = AlertService;
            TestingInputName = "Will Testing Item";

            FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, FileSystem);
        }

        [RelayCommand]
        public void OpenFile()
        {
            // TODO: open file logic
        }

        /// <summary>
        /// Retrieves the contents of a specified parent tree item.
        /// </summary>
        /// <param name="parent"></param>
        [RelayCommand]
        public void RetrieveContents(ITreeViewItem? parent)
        {
            Debug.WriteLine($"Retrieve file called. {parent is null}");
            if (parent == null)
            {
                FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, FileSystem);
                return;
            }
            if (parent is FolderTreeItem parentFolder)
            {
                FileStructureViewUtils.LoadFileSystemObjects(FileManagerService, parentFolder);
                return;
            }
            else
            {
                Debug.WriteLine($"Retrieve file called. Unknown parent type {parent is null}");
            }
        }

        [RelayCommand]
        public void AddFile(ITreeViewItem? parent)
        {
            Debug.WriteLine("Add File method called");

            // AddItem(parent, CreateFileBestFile);
            if (parent == null)
                return;

            // Will testing
            // Figure out the best way to handle the temp items being added
            // May need to explore implementing an abstract factory pattern or just regular factory patter
            // For now case parent into an IBNFolder
            // Really the type of this method hsould be IBNFolder, since that is what actually is having chilren
            // Alas for the current methods, I need the directory info parameters.
            // Need to work on this view model this weekend.

            // ((IBNFolder)parent).Children.Add(CreateTempTreeItem(parent.ItemLevel, parent.IndentationPadding, parent));
        }

        [RelayCommand]
        public void AddFolder(IBNFolder? parent)
        {
            // AddItem(parent, CreateFolderBestFile);
            if (parent == null)
                return;
        }
    }
}
