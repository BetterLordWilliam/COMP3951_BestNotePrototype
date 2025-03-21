﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BestNote_3951.Models.FileSystem;
using BestNote_3951.Services;
using Syncfusion.Pdf;

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
        private readonly FileManagerService fileManagerService;
        private readonly AlertService alertService;

        [ObservableProperty]
        public partial string TestingInputName { get; set; }

        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>

        public ObservableCollection<ITreeViewItem> FileSystem { get; private set;  } = new ObservableCollection<ITreeViewItem>();
        public ObservableCollection<string> FileNames { get; private set; } = new ObservableCollection<string>();

        public FileStructureViewModel(AlertService als, FileManagerService bfs)
        {
            // FileSystem = GenerateSource();
            fileManagerService = bfs;
            alertService = als;
            TestingInputName = "Will Testing Item";

            LoadFileSystemContents();
        }

        /// <summary>
        /// Load file system contents, initial method and on refresh.
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="parentPath"></param>
        private void LoadFileSystemContents(string folderName = "", string? parentPath = null)
        {
            FileSystem.Clear();

            var contents = fileManagerService.GetDirectoryInfoContents(folderName, parentPath);
            foreach (var item in contents)
            {
                FileSystem.Add(CreateTreeItem(item, 0, new Thickness(0)));
            }
        }
        
        /// <summary>
        /// Load the children of a specfic parent node, expand action on a IBNFolder item.
        /// </summary>
        /// <param name="parent"></param>
        private void LoadChildren(FolderTreeItem parent)
        {
            parent.Children.Clear();

            var contents = fileManagerService.GetDirectoryInfoContents(parent.DirectoryInfo.Name, parent.DirectoryInfo.Parent?.FullName);
            int childLevel = parent.ItemLevel + 1;
            Thickness childPadding = parent.IndentationPadding + new Thickness(20, 0, 0, 0);

            foreach (var item in contents)
            {
                parent.Children.Add(CreateTreeItem(item, childLevel, childPadding));
            }
        }

        private ITreeViewItem CreateTreeItem(FileSystemInfo fileSystemInfo, int itemLevel, Thickness indentationPadding)
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

        private ITreeViewItem CreateFileTreeItem(FileInfo fileInfo, int itemLevel, Thickness indentationPadding)
        {
            IBNFile bnFile = new MarkdownFile(fileInfo, fileManagerService);
            return new FileTreeItem(itemLevel, indentationPadding, bnFile);
        }

        private ITreeViewItem CreateFolderTreeItem(DirectoryInfo directoryInfo, int itemLevel, Thickness indentationPadding)
        {
            IBNFolder bnFolder = new WindowsFolder(directoryInfo, fileManagerService);
            return new FolderTreeItem(itemLevel, indentationPadding, bnFolder);
        }

        [RelayCommand]
        public void OpenFile()
        {
            // TODO: open file logic
        }

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
            // AddItem(parent, CreateFileBestFile);
            if (parent == null)
                return;
        }

        [RelayCommand]
        public void AddFolder(ITreeViewItem? parent)
        {
            // AddItem(parent, CreateFolderBestFile);
            if (parent == null)
                return;
        }
    }
}
