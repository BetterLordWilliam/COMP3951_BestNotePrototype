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

        private string _themeText;
        public string ThemeText
        {
            get => _themeText;
            set => SetProperty(ref _themeText, value);
        }

        public MainPageViewModel()
        {
            #region Services
            FileManagerService = new FileManagerService();
            AlertService = new AlertService();
            #endregion

            #region ViewModels
            FileStructureViewModel = new FileStructureViewModel(AlertService, FileManagerService);
            EmbeddedPdfViewModel = new EmbeddedPdfViewModel(FileManagerService);
            MarkdownEditorViewModel = new MarkdownEditorViewModel();
            MarkdownRendererViewModel = new MarkdownRendererViewModel();
            MainPanelViewModel = new MainPanelViewModel(EmbeddedPdfViewModel, MarkdownEditorViewModel, MarkdownRendererViewModel);
            #endregion

            if (Application.Current.UserAppTheme == AppTheme.Light)
            {
                _themeText = "Light";
            }
            else if (Application.Current.UserAppTheme == AppTheme.Dark)
            {
                _themeText = "Dark";
            }
            else
            {
                _themeText = "Switch Theme";
            }
        }

        [RelayCommand]
        private void SwitchTheme()
        {
            var currentTheme = Application.Current?.UserAppTheme;

            if (currentTheme == AppTheme.Light)
            {
                Application.Current.UserAppTheme = AppTheme.Dark;
                ThemeText = "Dark";
            }
            else
            {
                Application.Current.UserAppTheme = AppTheme.Light;
                ThemeText = "Light";
            }
        }
    }
}
