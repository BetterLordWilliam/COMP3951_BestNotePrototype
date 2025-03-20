using CommunityToolkit.Mvvm.ComponentModel;
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
        public string testingInputName;

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

        private void LoadFileSystemContents(string folderName = "", string? parentPath = null)
        {
            FileSystem.Clear(); // Clear existing items

            var contents = fileManagerService.GetDirectoryInfoContents(folderName, parentPath);
            foreach (var item in contents)
            {
                FileSystem.Add(CreateTreeItem(item, 0, new Thickness(0))); // Initial level and padding
            }
        }

        private void LoadChildren(FolderTreeItem parent)
        {
            parent.Children.Clear(); // Clear existing children

            var contents = fileManagerService.GetDirectoryInfoContents(parent.DirectoryInfo.Name, parent.DirectoryInfo.Parent?.FullName);
            int childLevel = parent.ItemLevel + 1;
            Thickness childPadding = parent.IndentationPadding + new Thickness(20, 0, 0, 0); // Adjust as needed

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

        /// <summary>
        /// Handles the open file logic (eventually)
        /// 
        /// The relay command attribute automatically generates a command property that can be invoked.
        /// It basically just implements a Command  design pattern with a decorator instead of having to write that crap ourselves.
        /// </summary>
        /// <param name="file"></param>
        [RelayCommand]
        public void OpenFile()
        {
            // TODO: open file logic
        }

        [RelayCommand]
        public void RetrieveContents(ITreeViewItem? parent)
        {
            if (parent == null)
            {
                Debug.WriteLine($"Retrieve file called. Parent null: {parent is null}");
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
        }

        [RelayCommand]
        public void AddFolder(ITreeViewItem? parent)
        {
            // AddItem(parent, CreateFolderBestFile);
        }
    }
}
