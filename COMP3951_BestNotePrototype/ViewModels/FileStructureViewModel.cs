using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using BestNote_3951.Models.FileSystem;
using BestNote_3951.Services;
using Syncfusion.Pdf;
using System.IO;
using MetalPerformanceShaders;

///
/// Will Otterbein
/// March 12 2025
/// 
namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// Handles logic for creating ITreeView items from file system objects.
    /// 
    /// A proxy for the services provided by the FileManagerService class.
    /// </summary>
    class FileStructureViewUtils
    {
        static readonly Dictionary<> FileSystemTypeMap = new Dictionary<>();

        /// <summary>
        /// Returns an observable collection of ITreeViewItems.
        /// Returns an empty singleton list if there are no children.
        /// A hard reload of the items inside of the specified directory,
        /// root directory if `TargetDirectory` is null.
        /// </summary>
        /// <param name="TargetCollection"></param>
        /// <param name="FileManagerService"></param>
        /// <param name="TargetDirectory"></param>
        /// <returns></returns>
        public static void LoadFileSystemObjects(
            ObservableCollection<ITreeViewItem> TargetCollection,
            FileManagerService                  FileManagerService,
            DirectoryInfo?                      TargetDirectory = null
        ) {
            try
            {
                TargetCollection.Clear();

                var contents = FileManagerService.GetDirectoryInfoContents(TargetDirectory?.FullName);
                foreach (var item in contents)
                {
                    TargetCollection.Add(CreateTreeViewItem(item));
                }
            }
            catch (Exception ex)
            {
                return (ObservableCollection<ITreeViewItem>)Enumerable.Empty<ITreeViewItem>();
            }
        }

        private static void CreateTreeViewFile()
        {

        }

        private static void CreateTreeViewFolder()
        {

        }

        /// <summary>
        /// Uses maps of file system types to tree view item implementations to instantiate tree view items.
        /// </summary>
        /// <param name="FileSystemInfo"></param>
        /// <returns></returns>
        public static ITreeViewItem CreateTreeViewItem(FileSystemInfo FileSystemInfo)
        {
            return null;
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

            FileStructureViewUtils.LoadFileSystemObjects(FileSystem, FileManagerService);
        }
        
        /// <summary>
        /// Load the children of a specfic parent node, expand action on a IBNFolder item.
        /// </summary>
        /// <param name="parent"></param>
        private void LoadChildren(FolderTreeItem parent)
        {
            parent.Children.Clear();

            var contents = FileManagerService.GetDirectoryInfoContents(parent.DirectoryInfo.Name, parent.DirectoryInfo.Parent?.FullName);
            int childLevel = parent.ItemLevel + 1;
            Thickness childPadding = parent.IndentationPadding + new Thickness(20, 0, 0, 0);

            foreach (var item in contents)
            {
                parent.Children.Add(CreateTreeItem(item, childLevel, childPadding));
            }
        }

        private ITreeViewItem CreateTreeItem(FileSystemInfo? fileSystemInfo, int itemLevel, Thickness indentationPadding, ITreeViewItem? parentItem = null)
        {
            if (fileSystemInfo is FileInfo fileInfo)
            {
                return CreateFileTreeItem(fileInfo, itemLevel, indentationPadding);
            }
            else if (fileSystemInfo is DirectoryInfo directoryInfo)
            {
                return CreateFolderTreeItem(directoryInfo, itemLevel, indentationPadding);
            }
            else
            {
                throw new ArgumentException("Unknown FileSystemInfo type");
            }
        }

        /// <summary>
        /// Method for renaming file tree itesm.
        /// </summary>
        /// <param name="Renameable"></param>
        private void RenameTreeItem(string newItemName, ITreeViewItem? Renameable)
        {
            // if (Renameable.)
        }

        /// <summary>
        /// Creates a file tree item at the specified level of the tree.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="itemLevel"></param>
        /// <param name="indentationPadding"></param>
        /// <returns></returns>
        private ITreeViewItem CreateFileTreeItem(FileInfo fileInfo, int itemLevel, Thickness indentationPadding)
        {
            IBNFile bnFile = new MarkdownFile(fileInfo, FileManagerService);
            return new FileTreeItem(itemLevel, indentationPadding, bnFile);
        }

        /// <summary>
        /// Creates a folder tree item at the specified level of the file tree.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="itemLevel"></param>
        /// <param name="indentationPadding"></param>
        /// <returns></returns>
        private FolderTreeItem CreateFolderTreeItem(DirectoryInfo directoryInfo, int itemLevel, Thickness indentationPadding)
        {
            IBNFolder bnFolder = new WindowsFolder(directoryInfo, FileManagerService);
            return new FolderTreeItem(itemLevel, indentationPadding, bnFolder);
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
                LoadFileSystemContents();
                return;
            }
            if (parent is FolderTreeItem parentFolder)
            {
                LoadChildren(parentFolder);
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
