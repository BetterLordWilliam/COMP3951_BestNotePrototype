using BestNote_3951.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Syncfusion.Maui.PdfViewer;

namespace BestNote_3951.Messages
{
    /// <summary>
    /// Sends a message from the PDFViewModel when a bookmark is created so that the 
    /// markdown editor can insert a link to it.
    /// </summary>
    public class PdfBookmarkTomarkdownMessage : ValueChangedMessage<ResourceLink>
    {
        //public PdfBookmarkTomarkdownMessage(Bookmark bookmark) : base(bookmark) { }

        public PdfBookmarkTomarkdownMessage(ResourceLink resource) : base(resource) { }
    }

    /// <summary>
    /// Sent when the custom PDF link is clicked in the rendered markdown.
    /// For now carries the pageNumber that the PDF viewer should navigate to.
    /// Any other properties that want to be sent with the link should be added to
    /// the constructor.
    /// </summary>
    public class MarkdownLinkClickedMessage : ValueChangedMessage<int>
    {
        public MarkdownLinkClickedMessage(int pageNumber) : base(pageNumber) { }
    }

    /// <summary>
    /// Sent when the custom PDF link is clicked in the rendered markdown.
    /// For now carries the pageNumber that the PDF viewer should navigate to.
    /// Any other properties that want to be sent with the link should be added to
    /// the constructor.
    /// </summary>
    public class MarkdownLinkClickedPathMessage : ValueChangedMessage<string>
    {
        public MarkdownLinkClickedPathMessage(string pdfPath) : base(pdfPath) { }
    }
}
