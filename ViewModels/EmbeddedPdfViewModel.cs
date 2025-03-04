using CommunityToolkit.Mvvm.ComponentModel;

namespace BestNote_3951.ViewModels
{
    public partial class EmbeddedPdfViewModel : ObservableObject
    {
        [ObservableProperty]
        private string pdfPath = "Enter PDF file path here.";
    }
}
