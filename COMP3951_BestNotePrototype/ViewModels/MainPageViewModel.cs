﻿using BestNote_3951.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BestNote_3951.Resources.Styles;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;

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
            MarkdownRendererViewModel = new MarkdownRendererViewModel();
            MarkdownEditorViewModel = new MarkdownEditorViewModel(AlertService);            
            MainPanelViewModel = new MainPanelViewModel(EmbeddedPdfViewModel, MarkdownEditorViewModel, MarkdownRendererViewModel);
            #endregion
        }


        [RelayCommand]
        private void SwitchTheme(string theme)
        {
            var mergedDictionaries = Application.Current!.Resources.MergedDictionaries;

            var toRemove = mergedDictionaries
                .Where(md => md is LightTheme || md is DarkTheme || md is BlueTheme)
                .ToList();

            foreach (var dict in toRemove)
            {
                mergedDictionaries.Remove(dict);
            }

            ResourceDictionary newTheme = theme switch
            {
                "Light" => new LightTheme(),
                "Dark" => new DarkTheme(),
                "Blue" => new BlueTheme(),
                "BlueDark" => new BlueThemeDark(),
                _ => new LightTheme() 
            };

            mergedDictionaries.Add(newTheme);

            WeakReferenceMessenger.Default.Send(new ThemeChangedMessage());
        }
    }
}
