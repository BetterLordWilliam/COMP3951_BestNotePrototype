using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// March 23 2025
/// 
namespace BestNote_3951.Models.FileSystem;

/// <summary>
/// Tree Folder Items.
/// </summary>
public partial class FolderTreeItem : TreeViewItemBase, IBNFolder
{
    private readonly IBNFolder _sourceFolder;

    /// <summary>
    /// File tree folder constructor.
    /// </summary>
    /// <param name="itemLevel"></param>
    /// <param name="indentationPadding"></param>
    /// <param name="sourceFolder"></param>
    public FolderTreeItem(
        int itemLevel,
        Thickness indentationPadding,
        IBNFolder sourceFolder
    )
    {
        _sourceFolder = sourceFolder;
        ItemName = sourceFolder.DirectoryInfo.Name;
        ImageIcon = "folder_icon.png";
        ItemLevel = itemLevel;
        IndentationPadding = indentationPadding;
    }

    // Define folder specific tree view item properties
    public override bool CanHaveChildren => true;
    public override bool HasChildren => _sourceFolder.Children.Count > 0;

    /// <summary>
    /// Directory info for the file system object this folder represents in BestNote.
    /// </summary>
    public DirectoryInfo DirectoryInfo
    {
        get => _sourceFolder.DirectoryInfo;
        set => _sourceFolder.DirectoryInfo = value;
    }

    /// <summary>
    /// Observable collection of child ITreeViewItems.
    /// </summary>
    public ObservableCollection<ITreeViewItem> Children
    {
        get => _sourceFolder.Children;
        set => _sourceFolder.Children = value;
    }

    /// <summary>
    /// Safe children implementations, returns the children of the item.
    /// </summary>
    public override IEnumerable<ITreeViewItem> SafeChildren
    {
        get => _sourceFolder.Children;
    }
}
