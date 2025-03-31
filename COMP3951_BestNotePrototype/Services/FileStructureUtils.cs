using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Models.FileSystem;

///
/// Will Otterbein
/// March 30, 2025
/// 
namespace BestNote_3951.Services
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
            Thickness? IndentationPadding = null,
            FolderTreeItem? Parent = null
        )
        {
            try
            {
                TargetCollection.Clear();

                var contents = FileManagerService.GetDirectoryInfoContents(TargetDirectory?.FullName);

                foreach (var item in contents)
                {
                    ITreeViewItem TreeItem = CreateTreeViewItem(FileManagerService, item, Level, IndentationPadding);
                    TreeItem.Parent = Parent;
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
        /// <param name="AlertManagerService"></param>
        /// <param name="TargetItem"></param>
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
                    FolderItem.IndentationPadding,
                    FolderItem
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
        )
        {
            // Map to extension

            var FileExtension = FileInfo.Extension.ToLower();

            Debug.WriteLine($"File system info object {FileInfo} w/ extension {FileExtension}");

            var MarkdownFile = new MarkdownFile(FileInfo, FileManagerService);
            var FileTreeItem = new FileTreeItem(FileManagerService, ItemLevel, IndentationPadding, MarkdownFile);

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
        )
        {
            // Default is windows folder

            Debug.WriteLine($"File system info object {DirectoryInfo}.");

            var WindowsFolder = new WindowsFolder(DirectoryInfo, FileManagerService);
            var FolderTreeItem = new FolderTreeItem(FileManagerService, ItemLevel, IndentationPadding, WindowsFolder);

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
        )
        {
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
}
