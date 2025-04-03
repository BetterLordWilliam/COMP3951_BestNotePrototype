using BestNote_3951.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// ViewModel for the main page where the file structure viewmodel and services are created.
    /// </summary>
    public partial class MainPageViewModel : ObservableObject
    {
        #region Services
        public AlertService AlertService { get; private set; }
        public FileManagerService FileManagerService { get; private set; }
        #endregion

        #region ViewModels
        public FileStructureViewModel FileStructureViewModel { get; private set; }
        public EmbeddedPdfViewModel EmbeddedPdfViewModel { get; private set; }
        public MainPanelViewModel MainPanelViewModel { get; private set; }
        public MarkdownEditorViewModel MarkdownEditorViewModel { get; private set; }
        public MarkdownRendererViewModel MarkdownRendererViewModel { get; private set; }
        #endregion

        public MainPageViewModel()
        {
            #region Services
            FileManagerService = new FileManagerService();
            AlertService = new AlertService();
            #endregion

            #region ViewModels
            FileStructureViewModel = new FileStructureViewModel(AlertService, FileManagerService);
            EmbeddedPdfViewModel = new EmbeddedPdfViewModel(AlertService, FileManagerService);
            MarkdownEditorViewModel = new MarkdownEditorViewModel();
            MarkdownRendererViewModel = new MarkdownRendererViewModel();
            MainPanelViewModel = new MainPanelViewModel(EmbeddedPdfViewModel, MarkdownEditorViewModel, MarkdownRendererViewModel);
            #endregion
        }

        [RelayCommand]
        private void SwitchTheme(string theme)
        {
            if (theme == "Light")
            {
                Application.Current!.UserAppTheme = AppTheme.Light;
            }
            else if (theme == "Dark")
            {
                Application.Current!.UserAppTheme = AppTheme.Dark;
            }
        }
    }
}
