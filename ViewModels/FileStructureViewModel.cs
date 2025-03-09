using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Syncfusion.Maui.TreeView;
using BestNote_3951.Models;
using BestNote_3951.Services;

namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// Handles the logic for the file structure view.
    /// 
    /// Observable object just simplifies a buttload of boilerplater code so we can use the observer pattern out of the box easily.
    /// </summary>
    public partial class FileStructureViewModel : ObservableObject
    {
        private FileManagerService fileManagerService;

        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>

        public ObservableCollection<BestFile> FileSystem { get; set; }

        public FileStructureViewModel(FileManagerService bfs)
        {
            FileSystem = GenerateSource();
            fileManagerService = bfs;
        }

        /// <summary>
        /// Testing method to generate items for the tree view model.
        /// </summary>
        private ObservableCollection<BestFile> GenerateSource()
        {
            var FileSystemTest = new ObservableCollection<BestFile>();

            var folder = new BestFile { ItemName = "MyNotes", ImageIcon = "folder_icon.png" };
            var folder1 = new BestFile { ItemName = "MyBetterNotes", ImageIcon = "folder_icon.png" };

            var doc = new BestFile { ItemName = "comp3951notes.md" , ImageIcon = "md_file.png" };
            var doc1 = new BestFile { ItemName = "comp3721notes.md" , ImageIcon = "md_file.png" };
            var doc2 = new BestFile { ItemName = "textbook.pdf" , ImageIcon = "md_file.png" };
            var doc3 = new BestFile { ItemName = "quakelogo.png", ImageIcon = "md_file.png" };
            var doc4 = new BestFile { ItemName = "comp3951notes.md", ImageIcon = "md_file.png" };
            var doc5 = new BestFile { ItemName = "comp3721notes.md", ImageIcon = "md_file.png" };
            var doc6 = new BestFile { ItemName = "textbook.pdf", ImageIcon = "md_file.png" };
            var doc7 = new BestFile { ItemName = "quakelogo.png", ImageIcon = "md_file.png" };

            var sub1 = new BestFile { ItemName = "SillyNotes.md" , ImageIcon = "md_file.png" };
            var sub2 = new BestFile { ItemName = "SeriousNodes.md", ImageIcon = "md_file.png" };
            folder.SubFiles = new ObservableCollection<BestFile>
            {
                sub1,
                sub2
            };

            FileSystemTest.Add(doc);
            FileSystemTest.Add(doc1);
            FileSystemTest.Add(folder1);
            FileSystemTest.Add(doc2);
            FileSystemTest.Add(doc3);
            FileSystemTest.Add(folder);
            FileSystemTest.Add(doc4);
            FileSystemTest.Add(doc5);
            FileSystemTest.Add(doc6);

            return FileSystemTest;
        }

        /// <summary>
        /// Handles the open file logic (eventually)
        /// 
        /// The relay command attribute automatically generates a command property that can be invoked.
        /// It basically just implements a Command  design pattern with a decorator instead of having to write that crap ourselves.
        /// </summary>
        /// <param name="file"></param>
        [RelayCommand]
        public void OpenFile(BestFile file)
        {
            // TODO: open file logic
        }

        [RelayCommand]
        public void AddFolder(BestFile file)
        {
            Debug.WriteLine("Add folder called. ");
            fileManagerService.AddFolder("Will Test");
        }

        /// <summary>
        /// TESTING METHOD FOR ADDING FILES, ONLY UI.
        /// </summary>
        [RelayCommand]
        public void AddFile()
        {
            Debug.WriteLine("Add file called. ");
            fileManagerService.AddFile("Will Test File");
            FileSystem.Add(new BestFile { ItemName = "💀💀💀💀", ImageIcon = "md_file.png"});
        }
    }
}
