using BestNote_3951.ViewModels;

namespace BestNote_3951.Views;
public partial class FileStructureView : ContentView
{
	public FileStructureView()
	{
		InitializeComponent();
		BindingContext = new FileStructureViewModel();
	}
}