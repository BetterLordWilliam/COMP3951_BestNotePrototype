using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using BestNote_3951.Models;
using Syncfusion.Maui.PdfViewer;
using Syncfusion.Maui.Popup;
using CommunityToolkit.Maui.Views;

/// <summary>
/// AUTHOR: Olivia Grace worked on the EmbeddedPdfViewModel file
/// SOURCES:
/// Followed this YouTube tutorial to open and display the PDF (see OpenDocument and PickAndShow)
/// https://www.youtube.com/watch?v=E_-g-GcQZRE&list=PL5IWFN3_TaPrE_3Y10N2XReOe57CpnMjy&index=6
/// 
/// Used the Syncfusion PDF Viewer documentation for .NET MAUI:
/// https://help.syncfusion.com/maui/pdf-viewer/getting-started
/// https://help.syncfusion.com/maui/pdf-viewer/custom-bookmark
/// </summary>
namespace BestNote_3951.ViewModels
{
    public partial class EmbeddedPdfViewModel : ObservableObject
    {

        /// <summary>
        /// Gets and sets the stream of the currently loaded PDF document. Has a binding relationship
        /// with the EmbeddedPdfView pdfViewer DocumentStream property.
        /// </summary>
        [ObservableProperty]
        public Stream? _pdfDocumentStream;


        /// <summary>
        /// Gets and sets the path of the Pdf document.
        /// </summary>
        public String PdfPath;


        /// <summary>
        /// Gets and sets the page number of the PDF. Has a two-way binding relationship with the 
        /// EmbeddedPdfView pdfViewer PageNumber property.
        /// </summary>
        [ObservableProperty]
        public int _pageNum;


        ///// <summary>
        ///// Gets and sets the page number of the PDF. Has a two-way binding relationship with the 
        ///// EmbeddedPdfView pdfViewer PageNumber property.
        ///// </summary>
        //[ObservableProperty]
        //public SfPopup _popup;


        /// <summary>
        /// A collection of ResourceLink objects.
        /// </summary>
        internal Collection<ResourceLink> ResourceLinks;
       

        /// <summary>
        /// Creates a new ResourceLink object and adds it to the ResourceLink collection for this
        /// PDF.
        /// </summary>
        [RelayCommand]
        internal void CreateResourceLink()
        {
            if (PdfPath != null && PdfPath != "")
            {
                int pageNumber = PageNum;
                String name = "BestNoteBookmark";
                ResourceLinks.Add(new ResourceLink(new Bookmark(name, pageNumber), PdfPath));              
            }

        }

        void OnButtonPressed(object sender, EventArgs args)
        {
            OpenDocument();
        }


        /// <summary>
	    /// Creates a file picker that allows the user to select a file chos
	    /// </summary>
        [RelayCommand]
	    async void OpenDocument()
	    {
		    FilePickerFileType pdfFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<String>>
		    {
			    {DevicePlatform.iOS, new[] {"public.pdf"} },
			    {DevicePlatform.Android, new[] {"application/pdf"} },
			    {DevicePlatform.WinUI, new[] {"pdf"} },
			    {DevicePlatform.MacCatalyst, new[] {"pdf"} }
		});
            PickOptions options = new()
            {
                PickerTitle = "Please select a PDF file",
                FileTypes = pdfFileType,
            };
		    await PickAndShow(options);
	    }


        /// <summary>
        /// Displays a FilePicker object for the user to select a pdf file from. Sets the
        /// PdfDocumentStream to the chosen PDF file and the PdfPath to the path of the 
        /// chosen PDF file.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    PdfPath = result.FullPath;
                    PdfDocumentStream = await result.OpenReadAsync();
                }
            }
            catch (Exception ex)
            {
                String message;
                if (ex != null && String.IsNullOrEmpty(ex.Message) == false)
                {
                    message = ex.Message;
                }
                else
                {
                    message = "File open failed";
                }
                Application.Current?.MainPage?.DisplayAlert("Error", message, "OK");
            }
        }


        /// <summary>
        /// Initializes PageNum to be 0, PdfPath to be an empty string, and ResourceLinks to be
        /// a new empty collection of ResourceLink objects.
        /// </summary>
        public EmbeddedPdfViewModel()
        {
            PageNum = 0;
            PdfPath = "";
            ResourceLinks = new Collection<ResourceLink>();
        }


    }
}

