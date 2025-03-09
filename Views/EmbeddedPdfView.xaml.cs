using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using BestNote_3951.ViewModels;
using Syncfusion.Maui.PdfViewer;

/// <summary>
/// Sources:
/// followed this tutorial on youtube:
/// https://www.youtube.com/watch?v=E_-g-GcQZRE&list=PL5IWFN3_TaPrE_3Y10N2XReOe57CpnMjy&index=6
/// </summary>
namespace BestNote_3951.Views;

/// <summary>
/// Contains functionality for opening a pdf from the user's file system.
/// </summary>
public partial class EmbeddedPdfView : ContentView
{
	public EmbeddedPdfView()
	{
		InitializeComponent();
		EmbeddedPdfViewModel BindingContext = new EmbeddedPdfViewModel();
        CustomizePDFToolbar();

    }

    /// <summary>
    /// Removes the printer, annotations, previous page, next page, and page layour icons from
    /// the Syncfusion PDF Viewer toolbar.
    /// </summary>
    private void CustomizePDFToolbar()
    {
        var printer = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Print");
        var annotations = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Annotations");
        var prevPage = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Previous page");
        var nextPage = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Next page");
        var pageLayout = pdfViewer.Toolbars?.GetByName("PrimaryToolbar")?.Items?.GetByName("Page layout mode");

        if (printer != null && annotations != null && prevPage != null && nextPage != null && pageLayout != null)
        {
            printer.IsVisible = false;
            annotations.IsVisible = false;
            prevPage.IsVisible = false;
            nextPage.IsVisible = false;
            pageLayout.IsVisible = false;
        }
    }

	
    private void Bookmarks_Clicked(object sender, EventArgs e)
    {
        pdfViewer.GoToBookmark(new Bookmark("olivia test", 2));

    }

}