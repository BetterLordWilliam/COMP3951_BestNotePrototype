using CommunityToolkit.Mvvm.Input;
using BestNote_3951.Models.FileSystem;
using BestNote_3951.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// March 23 2025
/// 
namespace BestNote_3951.ViewModels
{

    /// <summary>
    /// Tree view model object.
    /// </summary>
    public partial class BestFileTreeItemViewModel : ObservableObject
    {
        public ITreeViewItem TreeViewItem { get; private set; }
        public FileManagerService FileManagerService { get; private set; }

        /// <summary>
        /// Constructor for the BestFileTreeViewModel.
        /// </summary>
        /// <param name="TreeViewItem"></param>
        /// <param name="FileManagerService"></param>
        public BestFileTreeItemViewModel(
            ITreeViewItem       TreeViewItem,
            FileManagerService  FileManagerService
        ) {
            this.TreeViewItem       = TreeViewItem;
            this.FileManagerService = FileManagerService;
        }

        /// <summary>
        /// Creates a note file as a child of this item.
        /// This command should only be available if ITreeViewItem is capable of having children.
        /// </summary>
        [RelayCommand]
        public void CreateChildFile(string NewFileName)
        {
            try
            {
                if (!TreeViewItem.CanHaveChildren)
                    throw new NotImplementedException();

                // Use the FileManagerService to create a child file
                FileManagerService.CreateFile(NewFileName, ((IBNFolder)TreeViewItem).DirectoryInfo.FullName);
            }
            catch (NotImplementedException ex)
            {

            }
        }

        /// <summary>
        /// Rename an item in the tree view.
        /// </summary>
        /// <param name="Rename"></param>
        [RelayCommand]
        public void RenameItem(ITreeViewItem Rename)
        {
            Debug.WriteLine("Rename item command invoked.");
            try
            {
                // File
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine($"Error renaming the file tree item {ex}");
            }
        }

        /// <summary>
        /// Command to confirm the renaming of an item.
        /// </summary>
        /// <param name="Rename"></param>
        [RelayCommand]
        public void RenameItemConfirm(ITreeViewItem Rename)
        {
            Debug.WriteLine("Rename item confirm command invoked.");
        }
    }
}
