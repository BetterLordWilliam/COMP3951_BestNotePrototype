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

        [ObservableProperty]
        public string testingInputName;

        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>

        public ObservableCollection<BestFile> FileSystem { get; set; }
        public ObservableCollection<string> FileNames { get; set; }

        public FileStructureViewModel(FileManagerService bfs)
        {
            // FileSystem = GenerateSource();
            fileManagerService = bfs;

            FileSystem = new ObservableCollection<BestFile>();
            FileNames = new ObservableCollection<string>();

            TestingInputName = "Will Testing Item";
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
            BestFile? bf = fileManagerService.AddFolder(TestingInputName);
            if (bf is not null)
            {
                this.FileSystem.Add(bf);
                this.FileNames.Add(bf.itemDirectory.FullName);
            }
        }

        /// <summary>
        /// TESTING METHOD FOR ADDING FILES, ONLY UI.
        /// </summary>
        [RelayCommand]
        public void AddFile()
        {
            Debug.WriteLine("Add file called. ");
            BestFile? bf = fileManagerService.AddFile(TestingInputName);
            if (bf is not null)
            {
                this.FileSystem.Add(bf);
                this.FileNames.Add(bf.itemDirectory.FullName);
            }
            else
                Debug.WriteLine("File could not be added.");
        }
    }
}
