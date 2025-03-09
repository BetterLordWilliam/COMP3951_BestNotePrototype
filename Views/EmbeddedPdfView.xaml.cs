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
		EmbeddedPdfViewModel BindingContext = new EmbeddedPdfViewModel(this);
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

	Bookmark Bookmark { get; set; }

	public int getPageNumber()
	{
        Debug.WriteLine("hello");
		return 0;
		//return pdfViewer.PageNumber;

	}

	

	public void addBookmark(Bookmark newBookmark)
	{
		pdfViewer.CustomBookmarks.Add(newBookmark);
	}

    private void addLink_Clicked(object sender, EventArgs e)
    {
        int pageNum = pdfViewer.PageNumber;
        String name = "Olivia's bookmark yay";
        Bookmark myBookmark = new Bookmark(name, pageNum);
        pdfViewer.CustomBookmarks.Add(myBookmark);
    }


    ///// <summary>
    ///// Creates a file picker that allows the user to select a file chos
    ///// </summary>
    //async void OpenDocument()
    //{
    //	FilePickerFileType pdfFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<String>>
    //	{
    //		{DevicePlatform.iOS, new[] {"public.pdf"} },
    //		{DevicePlatform.Android, new[] {"application/pdf"} },
    //		{DevicePlatform.WinUI, new[] {"pdf"} },
    //		{DevicePlatform.MacCatalyst, new[] {"pdf"} }
    //	});
    //	PickOptions options = new()
    //	{
    //		PickerTitle = "Please select a PDF file",
    //		FileTypes = pdfFileType,
    //	};
    //	await PickAndShow(options);
    //}

    //public Stream PdfDocumentStream { get; set; }

    //public async Task PickAndShow(PickOptions options)
    //{
    //	try
    //	{
    //		var result = await FilePicker.Default.PickAsync(options);
    //		if (result != null)
    //		{
    //			PdfDocumentStream = await result.OpenReadAsync();
    //			this.pdfViewer.DocumentSource = PdfDocumentStream;
    //		}
    //	}
    //	catch (Exception ex)
    //	{
    //		String message;
    //		if (ex != null && String.IsNullOrEmpty(ex.Message) == false)
    //		{
    //			message = ex.Message;
    //		}
    //		else
    //		{
    //			message = "File open failed";
    //		}
    //		Application.Current?.MainPage?.DisplayAlert("Error", message, "OK");
    //	}
    //}

    //  private void openFile_Clicked(object sender, EventArgs e)
    //  {
    //OpenDocument();
    //  }
}