using BestNote_3951.Models;
using BestNote_3951.ViewModels;

namespace BestNote_3951.Views;

public partial class BestFileTreeView : ContentView
{
    public static readonly BindableProperty BestFileProperty =
		BindableProperty.Create(nameof(BestFile), typeof(BestFile), typeof(BestFileTreeView), propertyChanged: OnBestFileChanged);

    public BestFile BestFile
	{
		get { return (BestFile)GetValue(BestFileProperty);  }
		set { SetValue(BestFileProperty, value); }
	}

	public BestFileTreeView()
	{
        InitializeComponent();
	}

    private static void OnBestFileChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (BestFileTreeView)bindable;
        if (newValue is BestFile file)
        {
            view.BindingContext = file;
        }
    }

    private void OnToggleClicked(object sender, EventArgs e)
    {
        // Only invoke the command if the tree is not visible
        if (!SubFilesCollectionView.IsVisible && BestFile.Parent)
        {
            ((FileStructureViewModel)((FileStructureView)fileTreeItem.BindingContext).BindingContext).RetrieveContents(BestFile);
        }
        SubFilesCollectionView.IsVisible = !SubFilesCollectionView.IsVisible;
    }
}