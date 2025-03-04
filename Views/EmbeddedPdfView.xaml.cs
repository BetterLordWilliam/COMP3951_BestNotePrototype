using BestNote_3951.ViewModels;

namespace BestNote_3951.Views;

public partial class EmbeddedPdfView : ContentView
{
	public EmbeddedPdfView()
	{
		InitializeComponent();
		BindingContext = new EmbeddedPdfViewModel();
	}
}