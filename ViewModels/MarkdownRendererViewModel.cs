using CommunityToolkit.Mvvm.ComponentModel;

namespace BestNote_3951
{
    public partial class MarkdownRendererViewModel : ObservableObject
    {
        [ObservableProperty]
        private string renderedMarkdown = "rendered markdown";
    }
}
