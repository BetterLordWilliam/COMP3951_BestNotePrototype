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
using System.Text.RegularExpressions;

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
    /// <summary>
    /// ViewModel for displaying the PDF.
    /// 
    /// Has methods and properties for opening PDFs from the user's file system and navigating to specific
    /// pages within a PDF.
    /// </summary>
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
       

        /// <summary>
        /// Creates a new ResourceLink object and sends it to the views and view models that are registered
        /// to the PdfBookmarkTomarkdownMessage. This method is bound to the Add Link button.
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

                // sends to the registered messenger in the markdowneditor viewmodel constructor
                WeakReferenceMessenger.Default.Send(new PdfBookmarkTomarkdownMessage(resource));
            }
        }


        /// <summary>
        /// Creates a file picker that allows the user to select a PDF file from their file system, copy it to the
        /// BestNote Resources folder, and then display the copied file in the PDF viewer. This method is binded to
        /// the Copy to Resources button in the PDF View.
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
	    /// Creates a file picker that allows the user to select a file from their file system and display it in the
        /// PDF viewer. This method is binded to the No Thanks! button in the PDF View.
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
        /// Displays a FilePicker object for the user to select a PDF file from their file syste. If 
        /// copyToBestDirectory is True then the selected PDF is copied to the BestNote Resources folder and the
        /// copied file is used. PdfPath, PdfName, and PdfDocumentStream are all updated to the selected file.
        /// </summary>
        /// <param name="options">a PickOptions object for choosing a PDF file</param>
        /// <param name="copyToBestDirectory">a bool value indicating if the PDF should be copied to Resources</param>
        /// <returns>Task</returns>
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
        /// Displays the PDF passed as a file path in the PDF View.
        /// </summary>
        /// <param name="pdfPath">a String, the path of the PDF to display</param>
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
        /// Initializes PageNum to be 0, PdfPath and PdfName to be empty strings, and FileManagerService to be the 
        /// specified FileManagerService object. Registers the MarkdownLinkClickedPathMessage to open the correct
        /// PDF when a Markdown link is clicked.
        /// </summary>
        public EmbeddedPdfViewModel(FileManagerService bfs)
        {
            PageNum = 0;
            PdfPath = "";
            PdfName = "";
            fileManagerService = bfs;

            // Open the specified PDF
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

