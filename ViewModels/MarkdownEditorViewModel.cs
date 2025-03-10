using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;

namespace BestNote_3951.ViewModels
{
    public partial class MarkdownEditorViewModel : ObservableObject
    {
        // the text in the text editor is stored in this string. could be replaced with a different data structure if required
        [ObservableProperty]
        private string markdownText = "# Hello";

        // gets called automatically when markdown changes in hte editor pane
        partial void OnMarkdownTextChanged(string? oldValue, string newValue)
        {
            // send a message with the updated text
            WeakReferenceMessenger.Default.Send(new MarkdownTextChangedMessage(newValue));
        }
    }
}
