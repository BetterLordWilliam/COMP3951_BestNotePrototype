using CommunityToolkit.Mvvm.ComponentModel;

namespace BestNote_3951.ViewModels
{
    public partial class MarkdownEditorViewModel : ObservableObject
    {
        [ObservableProperty]
        private string markdownText = "type here yo";
    }
}
