using BestNote_3951.ViewModels;

namespace BestNote_3951.Views;

public partial class MarkdownEditorView : ContentView
{
	public MarkdownEditorView()
	{
		InitializeComponent();
		BindingContext = new MarkdownEditorViewModel();
	}
}