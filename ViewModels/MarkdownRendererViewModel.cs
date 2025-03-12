using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;
using Markdig;
using Microsoft.Maui.Controls;

/**
 * https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/mvvm?view=net-maui-9.0
 */
namespace BestNote_3951
{
    public partial class MarkdownRendererViewModel : ObservableObject
    {
        [ObservableProperty]
        private HtmlWebViewSource webViewSource;

        public MarkdownRendererViewModel()
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            string testConversion = Markdown.ToHtml("**Test Bold**", pipeline);
            Debug.WriteLine($"markdig test: {testConversion}");

            WebViewSource = new HtmlWebViewSource
            {
                Html = "<html><head><meta charset=\"utf-8\"></head><body></body></html>"
            };

            WeakReferenceMessenger.Default.Register<MarkdownTextChangedMessage>(this, (recipient, message) =>
            {         
                string text = message.Value;
                Debug.WriteLine($"received the message: {text}");

                // markdig the text 
                var htmlBody = Markdown.ToHtml(text, pipeline);
                var html = $"<html><head><meta charset=\"utf-8\"></head><body>{htmlBody}</body></html>";

                // force UI to update on hte main thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    WebViewSource = new HtmlWebViewSource { Html = html };
                    Debug.WriteLine($"please please please: {WebViewSource.Html}");
                });
            });
        }

    }
}
