using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using System.ComponentModel;

namespace BestNote_3951.ViewModels
{
    public partial class EmbeddedPdfViewModel : ObservableObject
    {
        //[ObservableProperty]
        //private string pdfPath = "Enter PDF file path here.";

        private Stream pdfDocumentStream;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the stream of the currently loaded PDF document.
        /// </summary>
        public Stream PdfDocumentStream
        {
            get
            {
                return pdfDocumentStream;
            }
            set
            {
                pdfDocumentStream = value;
                OnPropertyChanged(nameof(PdfDocumentStream));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfViewerViewModel"/> class.
        /// </summary>
        public EmbeddedPdfViewModel()
        {
            //InitializeComponent();
            //BindingContext = new FileStructureViewModel();
            // Load the embedded PDF document stream.
            pdfDocumentStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("C:\\Users\\olivi\\Documents\\CST_Term3\\COMP_3951\\3951_CourseOutline.pdf");
        }


    }
}

