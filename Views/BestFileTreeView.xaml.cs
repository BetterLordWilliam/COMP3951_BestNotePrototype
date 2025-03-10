using BestNote_3951.Models;

namespace BestNote_3951.Views;

public partial class BestFileTreeView : ContentView
{
    public static readonly BindableProperty BestFileProperty =
		BindableProperty.Create(nameof(BestFile), typeof(BestFile), typeof(BestFileTreeView));

    public BestFile BestFile
	{
		get { return (BestFile)GetValue(BestFileProperty);  }
		set { SetValue(BestFileProperty, value); }
	}

	public BestFileTreeView()
	{
		InitializeComponent();
	}
}