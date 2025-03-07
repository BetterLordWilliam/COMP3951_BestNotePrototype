using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace BestNote_3951.ViewModels
{
    public partial class EmbeddedPdfViewModel : ObservableObject
    {
        //[ObservableProperty]
        //private string pdfPath = "Enter PDF file path here.";

        //[ObservableProperty]
        //private Stream pdfDocumentStream;

        /// <summary>
        /// Gets or sets the stream of the currently loaded PDF document.
        /// </summary>
        [ObservableProperty]
        public Stream _pdfDocumentStream;

        //public ICommand OpenPdfCommand { get; private set; }


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
        public EmbeddedPdfViewModel()
        { 
            
        }


    }
}

