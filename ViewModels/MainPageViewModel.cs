using BestNote_3951.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BestNote_3951.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        #region Services
        public FileManagerService FileManagerService { get; private set; }
        #endregion

        #region ViewModels
        public FileStructureViewModel FileStructureViewModel { get; private set; }
        #endregion

        public MainPageViewModel()
        {
            #region Services
            FileManagerService = new FileManagerService();
            #endregion

            #region ViewModels
            FileStructureViewModel = new FileStructureViewModel(FileManagerService);
            #endregion
        }

    }
}
