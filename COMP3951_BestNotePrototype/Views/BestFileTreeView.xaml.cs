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

    /// <summary>
    /// Event handler for file tree items being clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnToggleClicked(object sender, EventArgs e)
    {
        // Only invoke the command if the tree is not visible
        if (!SubFilesCollectionView.IsVisible && TreeViewItem.CanHaveChildren && TreeViewItem is FolderTreeItem FolderTreeItem)
        {
            Debug.WriteLine("Clicked and condition validated.");
            ((FileStructureViewModel)((FileStructureView)fileTreeItem.BindingContext).BindingContext).RetrieveContents(FolderTreeItem);
        }
        SubFilesCollectionView.IsVisible = !SubFilesCollectionView.IsVisible;
    }
    
    /// <summary>
    /// Event handler for the rename context button being clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnRenameClicked(object sender, EventArgs e)
    {
        if (!TreeViewItem.CanRename)
            return;

        // Only invoke the command if the item can be renamed
        if (!fileTreeRename.IsVisible)
        {
            Debug.WriteLine("Rename clicked.");
            fileTreeRename.IsVisible = true;
            fileTreeName.IsVisible = false;
            fileTreeRename.Focus();
            fileTreeRename.Completed += OnRenameCompleted;

            // ((FileStructureViewModel)((FileStructureView)fileTreeItem.BindingContext).BindingContext).RetrieveContents(TreeViewItem);
        }
    }

    private void OnRenameCompleted(object sender, EventArgs e)
    {
        var entry = (Entry)sender;
        string newName = entry.Text;
        
        // Cannot rename an item with an empty name
        if (string.IsNullOrWhiteSpace(newName))
        {
            ((FileStructureViewModel)((FileStructureView)fileTreeItem.BindingContext).BindingContext).ShowAlertMessage("Item name cannot be empty.");
            return;
        }

        // Process the renamed item (e.g., update the data model)
        // ... your renaming logic here ...
        Debug.WriteLine($"Item renamed to: {newName}");

        //Unsubscribe from the event, prevents multiple calls.
        entry.Completed -= OnRenameCompleted;

        // Hide the Entry and show the original Label
        fileTreeRename.IsVisible = false;
        fileTreeName.IsVisible = true;
    }
}