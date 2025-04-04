﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;
using Syncfusion.Maui.PdfViewer;
using BestNote_3951.Models;

/**
 * Authors: Bryson Lindy
 *          Olivia Grace - worked on part of the Pdf link messaging
 */
namespace BestNote_3951.ViewModels
{
    /// <summary>
    /// ViewModel for the Markdown editor. 
    /// 
    /// Houses the markdown text property that is sent via the OnMarkdownTextChanged
    /// property to any ViewModel that has been registerd to the event.
    /// </summary>
    public partial class MarkdownEditorViewModel : ObservableObject
    {
        /// <summary>
        /// Property for the markdown text to be rendered.
        /// </summary>
        [ObservableProperty]
        private string markdownText = "# Hello";

        public MarkdownEditorViewModel()
        {
            WeakReferenceMessenger.Default.Register<PdfBookmarkTomarkdownMessage>(this, (recipient, message) =>
            {
                ResourceLink resource = message.Value;
                String pdfPath        = resource.ResourcePath;
                Bookmark bookmark     = resource.ResourceBookmark;
                String pdfName        = bookmark.Name;
                string link           = $"[{pdfName} (Page {bookmark.PageNumber})](<bestnote://bookmark?pg={bookmark.PageNumber}&pdf={pdfPath}>)";

                WeakReferenceMessenger.Default.Send(new InsertAtCursorMessage(link));
            });

            WeakReferenceMessenger.Default.Send(new MarkdownTextChangedMessage(markdownText));
        }

        /// <summary>
        /// Event generated by the MVVM community toolkit Observable property that allows us
        /// to communicate across Views/ViewModels. This is sent to anything that is registered
        /// with the markdown changing.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        partial void OnMarkdownTextChanged(string? oldValue, string newValue)
        {
            WeakReferenceMessenger.Default.Send(new MarkdownTextChangedMessage(newValue));
        }
    }
}
