using BestNote_3951.ViewModels;

namespace BestNote_3951.Views
{
    /// <summary>
    /// Connects the UI to it's corresponding ViewModel.
    /// Sets the binding context to an instance of the ViewModel so the UI can access it
    /// </summary>
    public partial class FileStructureView : ContentView
    {
        public FileStructureView()
        {
            InitializeComponent();
            BindingContext = new FileStructureViewModel();
        }

        /// <summary>
        /// This is triggered whenever a file is selected in the CollectionView from FileStructureView.
        /// 
        /// It calls open file from the ViewModel to handle the file opening logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // makes sure we're selecting a BestFile
            if (e.CurrentSelection.FirstOrDefault() is Models.BestFile selectedFile)
            {
                // passes the selected file to the OpenFile command
                ((FileStructureViewModel)BindingContext).OpenFileCommand.Execute(selectedFile);
            }
        }
    }
}
