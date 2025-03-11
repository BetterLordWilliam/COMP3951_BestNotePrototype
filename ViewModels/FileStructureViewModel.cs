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
        private AlertService alertService;

        [ObservableProperty]
        public string testingInputName;

        [ObservableProperty]
        public BestFile selectedParent;

        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>

        public ObservableCollection<BestFile> FileSystem { get; } = new ObservableCollection<BestFile>();
        public ObservableCollection<string> FileNames { get; } = new ObservableCollection<string>();

        public FileStructureViewModel(AlertService als, FileManagerService bfs)
        {
            // FileSystem = GenerateSource();
            fileManagerService = bfs;
            alertService = als;

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
        public void AddFolder(BestFile? parent = null)
        {
            Debug.WriteLine("Add folder called. ");

            try
            {
                if (string.IsNullOrEmpty(TestingInputName))
                    throw new Exception("Empty item name");

                BestFile? bf = (parent is not null)
                    ? fileManagerService.AddFolder(TestingInputName, parentPath: parent.ItemDirectory.FullName)
                    : fileManagerService.AddFolder(TestingInputName);

                if (bf is null)
                    return;

                if (parent is not null)
                {
                    bf.Level = parent.Level + 10;
                    parent.SubFiles.Add(bf);
                }
                else
                {
                    FileSystem.Add(bf);
                }
                TestingInputName = "";
            }
            catch (Exception e)
            {
                string message = "Your action could not be complete.\n\nFolder or file name cannot be empty.";
#if DEBUG
                message = $"Your action could not be complete\n\n{e}";
#endif

                alertService.ShowAlertAsync("Error", message);
                Debug.WriteLine($"Exception occured {e}");
            }
        }

        /// <summary>
        /// TESTING METHOD FOR ADDING FILES, ONLY UI.
        /// </summary>
        [RelayCommand]
        public void AddFile(BestFile? parent = null)
        {
            Debug.WriteLine("Add file called. ");

            try
            {
                if (string.IsNullOrEmpty(TestingInputName))
                    throw new Exception("Empty item name");

                BestFile? bf = (parent is not null)
                    ? fileManagerService.AddFile(TestingInputName, parentPath: parent.ItemDirectory.FullName)
                    : fileManagerService.AddFile(TestingInputName);

                if (bf is null)
                    return;

                if (parent is not null)
                {
                    bf.Level = parent.Level + 10;
                    parent.SubFiles.Add(bf);
                }
                else
                {
                    FileSystem.Add(bf);
                }
                TestingInputName = "";
            }
            catch (Exception e)
            {
                string message = "Your action could not be complete.\n\nFolder or file name cannot be empty.";
#if DEBUG
                message = $"Your action could not be complete\n\n{e}";
#endif

                alertService.ShowAlertAsync("Error", message);
                Debug.WriteLine($"Exception occured {e}");
            }
        }
    }
}
