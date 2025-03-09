using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using BestNote_3951.Models;
using Syncfusion.Maui.PdfViewer;


namespace BestNote_3951.ViewModels
{
    public partial class EmbeddedPdfViewModel : ObservableObject
    {
        //[ObservableProperty]
        //private string pdfPath = "Enter PDF file path here.";

        //[ObservableProperty]
        //private Stream pdfDocumentStream;

        private BestNote_3951.Views.EmbeddedPdfView _embeddedPdfView;

        /// <summary>
        /// Gets or sets the stream of the currently loaded PDF document.
        /// </summary>
        [ObservableProperty]
        public Stream _pdfDocumentStream;

        /// <summary>
        /// Gets or sets the stream of the currently loaded PDF document.
        /// </summary>
        public String PdfPath;

        //public ICommand OpenPdfCommand { get; private set; }


        //internal ObservableCollection<Bookmark> Bookmarks { get; } = new ObservableCollection<Bookmark>();

       

        [RelayCommand]
        internal void CreateResourceLink()
        {
            if (PdfPath != null)
            {
                //Debug.WriteLine("in create resource link");
                int pageNumber = _embeddedPdfView.getPageNumber();
                //String name = "My Bookmark";
                //_embeddedPdfView.addBookmark(new Bookmark(name, pageNumber));
                //Bookmarks.Add(new Bookmark(name, pageNumber));
                
            }

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

        public async Task PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    PdfPath = result.FullPath;
                    //Console.WriteLine("file " + filename);
                    PdfDocumentStream = await result.OpenReadAsync();
                    //this.pdfViewer.DocumentSource = PdfDocumentStream;
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
        /// Initializes a new instance of the <see cref="PdfViewerViewModel"/> class.
        /// </summary>
        public EmbeddedPdfViewModel(BestNote_3951.Views.EmbeddedPdfView myView)
        {
            _embeddedPdfView = myView;
        }

        public EmbeddedPdfViewModel()
        {

        }


    }
}

