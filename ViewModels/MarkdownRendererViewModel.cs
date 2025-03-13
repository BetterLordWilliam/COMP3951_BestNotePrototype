using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;
using Markdig;
using Microsoft.Maui.Controls;

/*
 * Sources:
 * The data binding fundamentals were very helpful in getting data binding between the view and viewmodels
* https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/mvvm?view=net-maui-9.0
* But not nearly as helpful as this WPF profressional going over the MVVM community toolkit for the
* Avalonia framework. It really helped show how the Observable Properties and Objects should be connected
* across views/viewmodels.
* https://www.youtube.com/watch?v=bCryIp9HqIM
*/

namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// MarkdownRenderer ViewModel
    /// 
    /// This defines the logic that displays the rendered markdown in the MarkdownView file.
    /// Registers the viewmodel with the MarkdownTextChanged messaged so that it can receive real-time
    /// updates for the text change in the MarkdownEditor. 
    /// 
    /// Uses a webviewsource to display the rendered markdown.
    /// 
    /// </summary>
    public partial class MarkdownRendererViewModel : ObservableObject
    {
        /// <summary>
        /// Property for the webviewsource that displays the rendered markdown.
        /// </summary>
        [ObservableProperty]
        private HtmlWebViewSource webViewSource;

        public MarkdownRendererViewModel()
        {
            // pipeline gives access to advanced markdown rendering features.
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            WebViewSource = new HtmlWebViewSource
            {
                Html = "<html><head><meta charset=\"utf-8\"></head><body></body></html>"
            };

            WeakReferenceMessenger.Default.Register<MarkdownTextChangedMessage>(this, (recipient, message) =>
            {         
                string text = message.Value;
                //Debug.WriteLine($"received the message: {text}");

                // markdig the text 
                var htmlBody = Markdown.ToHtml(text, pipeline);
                var html = $"<html><head><meta charset=\"utf-8\"></head><body>{htmlBody}</body></html>";

                // force UI to update on hte main thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    WebViewSource = new HtmlWebViewSource { Html = html };
                    //Debug.WriteLine($"source HTML: {WebViewSource.Html}");
                });
            });
        }

    }
}
