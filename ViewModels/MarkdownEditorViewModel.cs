using CommunityToolkit.Mvvm.ComponentModel;

namespace BestNote_3951.ViewModels
{
    public partial class MarkdownEditorViewModel : ObservableObject
    {
        // the text in the text editor is stored in this string. could be replaced with a different data structure if required
        [ObservableProperty]
        private string markdownText = "";
    }
}
