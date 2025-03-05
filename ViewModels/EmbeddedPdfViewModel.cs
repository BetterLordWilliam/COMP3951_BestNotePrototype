using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using System.ComponentModel;

namespace BestNote_3951.ViewModels
{
    public partial class EmbeddedPdfViewModel : INotifyPropertyChanged
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

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property name.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

