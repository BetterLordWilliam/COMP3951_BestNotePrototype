using BestNote_3951.ViewModels;

namespace BestNote_3951.Views
{
    public partial class FileStructureView : ContentView
    {
        public FileStructureView()
        {
            InitializeComponent();
            BindingContext = new FileStructureViewModel();
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Models.BestFile selectedFile)
            {
                ((FileStructureViewModel)BindingContext).OpenFileCommand.Execute(selectedFile);
            }
        }
    }
}
