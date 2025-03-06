using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Syncfusion.Maui.TreeView;
using BestNote_3951.Models;

namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// Handles the logic for the file structure view.
    /// 
    /// Observable object just simplifies a buttload of boilerplater code so we can use the observer pattern out of the box easily.
    /// </summary>
    public partial class FileStructureViewModel : ObservableObject
    {
        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>

        private ObservableCollection<BestFile> fileSystem;

        public ObservableCollection<BestFile> FileSystem
        {
            get => fileSystem;
            set => SetProperty(ref fileSystem, value);
        }

        public ICommand AddFileCommand { get; private set; }

        public FileStructureViewModel()
        {
            AddFileCommand = new Command(() => FileSystem.Add(new BestFile { ItemName = "💀💀💀💀", ImageIcon = "md_file.png" }));
            fileSystem = GenerateSource();
        }

        /// <summary>
        /// Testing method to generate items for the tree view model.
        /// </summary>
        private ObservableCollection<BestFile> GenerateSource()
        {
            var fileSystemTest = new ObservableCollection<BestFile>();

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

            fileSystemTest.Add(doc);
            fileSystemTest.Add(doc1);
            fileSystemTest.Add(folder1);
            fileSystemTest.Add(doc2);
            fileSystemTest.Add(doc3);
            fileSystemTest.Add(folder);
            fileSystemTest.Add(doc4);
            fileSystemTest.Add(doc5);
            fileSystemTest.Add(doc6);

            return fileSystemTest;
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

        /// <summary>
        /// TESTING METHOD FOR ADDING FILES, ONLY UI.
        /// </summary>
        public void AddFile()
        {
            Console.WriteLine("Called ");
            // 🙂
            FileSystem.Add(new BestFile { ItemName = "💀💀💀💀", ImageIcon = "md_file.png"});
        }
    }
}
