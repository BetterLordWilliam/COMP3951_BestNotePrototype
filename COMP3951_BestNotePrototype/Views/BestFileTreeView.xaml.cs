using BestNote_3951.Models.FileSystem;
using BestNote_3951.ViewModels;

namespace BestNote_3951.Views;

public partial class BestFileTreeView : ContentView
{
	public BestFileTreeView()
	{
        InitializeComponent();
	}
    
    /// <summary>
    /// Event handler for the rename context button being clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnRenameClicked(object sender, EventArgs e)
    {
        if (!((BestFileTreeItemViewModel)BindingContext).TreeViewItem.CanRename)
            return;

        // Only invoke the command if the item can be renamed
        if (!fileTreeRename.IsVisible)
        {
            Debug.WriteLine("Rename clicked.");
            fileTreeRename.IsVisible = true;
            fileTreeName.IsVisible = false;
            fileTreeRename.Focus();
            fileTreeRename.Completed += OnRenameComplete;
        }
    }

    /// <summary>
    /// Event handler for the rename completed action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnRenameComplete(object sender, EventArgs e)
    {
        if (sender is Entry renameEntry)
        {
            Debug.WriteLine($"Item renamed to {renameEntry.Text}");
            fileTreeRename.IsVisible = false;
            fileTreeName.IsVisible = true;
            fileTreeRename.Completed -= OnRenameComplete;
        }
    }

    /// <summary>
    /// Event handler for the dragged over action.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DraggedOver(object sender, DragEventArgs e)
    {

    }
}