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
        private readonly FileManagerService fileManagerService;
        private readonly AlertService alertService;

        private delegate BestFile? CreateBestFile(BestFile? parent);

        [ObservableProperty]
        public string testingInputName;

        /// <summary>
        /// Files property is an ObservableCollection of BestFiles. ObservableCollection is part of the MVVM toolkit and it 
        /// allows the View to automatically be notified when items are added/removed/updated.
        /// </summary>

        public ObservableCollection<BestFile> FileSystem { get; private set;  } = new ObservableCollection<BestFile>();
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
        /// Load files/folders from the system.
        /// </summary>
        /// <param name="bfParent"></param>
        private void LoadFileSystemContents(BestFile? bfParent = null)
        {
            try
            {
                // Get details of where the file should go
                ObservableCollection<BestFile> contents;
                ObservableCollection<BestFile> destination;
                if (bfParent is null)
                {
                    contents = fileManagerService.GetFolderContents();
                    FileSystem = new ObservableCollection<BestFile>();
                    destination = FileSystem;
                }
                else
                {
                    contents = fileManagerService.GetFolderContents(parentPath: bfParent.DirectoryInfo.FullName);
                    bfParent.SubFiles = new ObservableCollection<BestFile>();
                    destination = bfParent.SubFiles;
                }
                if (contents is null)
                    return;

                // Add to either filesystem toplevel or subfiles
                foreach (BestFile bf in contents)
                {
                    bf.Level = bfParent?.Level + 10 ?? 0; 
                    destination.Add(bf);
                }
            }
            catch (Exception e)
            {
                string message = "Startup action failed.\n\nSystem contents could not be loaded.";
#if DEBUG
                message = $"Startup action failed.\n\n{e}\n\nSystem contents could not be loaded.";
#endif
                alertService.ShowAlertAsync("Error", message);
                TestingInputName = "";
                Debug.WriteLine($"Exception occured {e}");
            }
        }

        private void AddItem(BestFile? bfParent, CreateBestFile cbf)
        {
            Debug.WriteLine("Add item called. ");

            try
            {
                if (string.IsNullOrEmpty(TestingInputName))
                    throw new Exception("Empty item name");

                BestFile? bf = cbf(bfParent);

                if (bf is null)
                    return;

                if (bfParent is not null)
                {
                    bf.Level = bfParent.Level + 10;
                    bfParent.SubFiles.Add(bf);
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
                TestingInputName = "";
                Debug.WriteLine($"Exception occured {e}");
            }
        }

        private BestFile? CreateFolderBestFile(BestFile? bfParent)
        {
            return (bfParent is not null)
                ? fileManagerService.AddFolder(TestingInputName, parentPath: bfParent.ItemDirectory.FullName)
                : fileManagerService.AddFolder(TestingInputName);
        }

        private BestFile? CreateFileBestFile(BestFile? bfParent)
        {
            return (bfParent is not null)
                ? fileManagerService.AddFile(TestingInputName, parentPath: bfParent.ItemDirectory.FullName)
                : fileManagerService.AddFile(TestingInputName);
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
        public void RetrieveContents(BestFile? parent)
        {
            Debug.WriteLine("Retrieve file called.");
            if (parent is null)
                return;
            LoadFileSystemContents(parent);
        }

        [RelayCommand]
        public void AddFile(BestFile? parent)
        {
            AddItem(parent, CreateFileBestFile);
        }

        [RelayCommand]
        public void AddFolder(BestFile? parent)
        {
            AddItem(parent, CreateFolderBestFile);
        }
    }
}
