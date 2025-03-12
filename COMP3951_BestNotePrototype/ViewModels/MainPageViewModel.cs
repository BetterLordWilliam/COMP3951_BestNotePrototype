using BestNote_3951.Services;
using CommunityToolkit.Mvvm.ComponentModel;

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
        #endregion

        public MainPageViewModel()
        {
            #region Services
            FileManagerService = new FileManagerService();
            AlertService = new AlertService();
            #endregion

            #region ViewModels
            FileStructureViewModel = new FileStructureViewModel(AlertService, FileManagerService);
            #endregion
        }
    }
}
