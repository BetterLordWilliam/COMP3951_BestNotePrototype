using BestNote_3951.Models.FileSystem;
using BestNote_3951.ViewModels;

namespace BestNote_3951.Views;

public partial class BestFileTreeView : ContentView
{
    public static readonly BindableProperty TreeViewItemProperty =
		BindableProperty.Create(nameof(TreeViewItem), typeof(ITreeViewItem), typeof(BestFileTreeView), propertyChanged: OnBestFileChanged);

    public ITreeViewItem TreeViewItem
	{
		get { return (ITreeViewItem)GetValue(TreeViewItemProperty);  }
		set { SetValue(TreeViewItemProperty, value); }
	}

	public BestFileTreeView()
	{
        InitializeComponent();
	}

    private static void OnBestFileChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (BestFileTreeView)bindable;
        if (newValue is ITreeViewItem file)
        {
            view.BindingContext = file;
        }
    }

    private void OnToggleClicked(object sender, EventArgs e)
    {

        // Only invoke the command if the tree is not visible
        if (!SubFilesCollectionView.IsVisible && TreeViewItem.CanHaveChildren)
        {
            Debug.WriteLine("Clicked and condition validated.");
            ((FileStructureViewModel)((FileStructureView)fileTreeItem.BindingContext).BindingContext).RetrieveContents(TreeViewItem);
        }
        SubFilesCollectionView.IsVisible = !SubFilesCollectionView.IsVisible;
    }
}