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
using BestNote_3951.Services;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;

/// <summary>
/// AUTHOR: Olivia Grace worked on the EmbeddedPdfViewModel file
///         Bryson Lindy pdfviewmodel - markdown renderer messaging
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

        private readonly FileManagerService fileManagerService;

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
        /// Gets and sets the name of the Pdf document.
        /// </summary>
        public String PdfName;


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

            if (!string.IsNullOrEmpty(PdfPath))
            {
                int pageNumber    = PageNum;
                String name       = PdfName;
                Bookmark bookmark = new Bookmark(name, pageNumber);

                ResourceLink resource = new ResourceLink(new Bookmark(name, pageNumber), PdfPath);

                ResourceLinks.Add(new ResourceLink(new Bookmark(name, pageNumber), PdfPath));

                // sends to the registered messenger in the markdowneditor viewmodel constructor
                WeakReferenceMessenger.Default.Send(new PdfBookmarkTomarkdownMessage(resource));
            }
        }


        /// <summary>
        /// Creates a file picker that allows the user to select a file chos
        /// </summary>
        [RelayCommand]
        async void OpenDocumentAndCopy()
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
            await PickAndShow(options, true);
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
		    await PickAndShow(options, false);
	    }


        /// <summary>
        /// Displays a FilePicker object for the user to select a pdf file from. Sets the
        /// PdfDocumentStream to the chosen PDF file and the PdfPath to the path of the 
        /// chosen PDF file.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task PickAndShow(PickOptions options, bool copyToBestDirectory)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null && !copyToBestDirectory)
                {
                    PdfPath = result.FullPath;
                    PdfName = result.FileName;
                    PdfDocumentStream = await result.OpenReadAsync();
                } else if (result != null && copyToBestDirectory)
                {
                    
                    String newFilePath = fileManagerService.AddResourceFile(result.FileName, result.FullPath);
                    PdfName = Path.GetFileName(newFilePath);
                    OpenPDFFromPath(newFilePath);
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
        /// Displays the pdf passed as a file path into the
        /// </summary>
        /// <param name="pdfPath"></param>
        public void OpenPDFFromPath(String pdfPath)
        {
            try
            {
                PdfPath = pdfPath;
                PdfDocumentStream = File.OpenRead(pdfPath);
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
        public EmbeddedPdfViewModel(FileManagerService bfs)
        {
            PageNum = 0;
            PdfPath = "";
            PdfName = "";
            ResourceLinks = new Collection<ResourceLink>();
            fileManagerService = bfs;

            WeakReferenceMessenger.Default.Register<MarkdownLinkClickedPathMessage>(this, (recipient, message) =>
            {
                string filePath = message.Value;

                if (filePath != PdfPath)
                {
                    PdfName = Path.GetFileName(filePath);
                    PdfPath = filePath;
                    PdfDocumentStream = File.OpenRead(message.Value);
                    Thread.Sleep(50);
                }
            });
        }

    }
}

