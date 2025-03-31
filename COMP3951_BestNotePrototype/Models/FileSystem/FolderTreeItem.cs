using BestNote_3951.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// March 30 2025
/// 
namespace BestNote_3951.Models.FileSystem;

/// <summary>
/// Tree Folder Items.
/// </summary>
public partial class FolderTreeItem : TreeViewItemBase, IBNFolder
{
    private readonly IBNFolder _sourceFolder;
    private readonly FileManagerService _fileManagerService;

    /// <summary>
    /// File tree folder constructor.
    /// </summary>
    /// <param name="FileManagerService"></param>
    /// <param name="itemLevel"></param>
    /// <param name="indentationPadding"></param>
    /// <param name="sourceFolder"></param>
    public FolderTreeItem(
        FileManagerService  FileManagerService,
        int                 itemLevel,
        Thickness           indentationPadding,
        IBNFolder           sourceFolder
    )
    {
        _sourceFolder = sourceFolder;
        _fileManagerService = FileManagerService;

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
    public ObservableCollection<BestFileTreeItemViewModel> Children
    {
        get => _sourceFolder.Children;
        set => _sourceFolder.Children = value;
    }

    /// <summary>
    /// Adds a tree item to the nodes collection of children.
    /// </summary>
    /// <param name="NewChild"></param>
    public void AddChild(BestFileTreeItemViewModel NewChild)
    {
        int ChildLevel = (ItemLevel == 0) ? 0 : ItemLevel + 10;
        Thickness ChildIndentationPadding = IndentationPadding + new Thickness(10, 0, 0, 0);

        NewChild.TreeViewItem.ItemLevel = ChildLevel;
        NewChild.TreeViewItem.IndentationPadding = ChildIndentationPadding;

        _sourceFolder.AddChild(NewChild);
    }
    
    /// <summary>
    /// Removes a target folder from the nodes collection of children.
    /// </summary>
    /// <param name="TargetChild"></param>
    public void RemoveChild(BestFileTreeItemViewModel TargetChild)
    {
        _sourceFolder.RemoveChild(TargetChild);
    }

    /// <summary>
    /// Renames a folder item.
    /// </summary>
    /// <param name="NewItemName"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Rename(string NewItemName)
    {
        _sourceFolder.Rename(NewItemName);
        ItemName = _sourceFolder.DirectoryInfo.Name;
    }

    /// <summary>
    /// Move this folder item to a new parent.
    /// </summary>
    /// <param name="NewParent"></param>
    public override void Move(FolderTreeItem NewParent)
    {
        // Use the file manager service to move the item
        DirectoryInfo NewDirInfo = _fileManagerService.MoveFolder(DirectoryInfo, NewParent.DirectoryInfo);
        DirectoryInfo = NewDirInfo;
        Parent = NewParent;
    }

    /// <summary>
    /// Load the objects 
    /// </summary>
    public void LoadFileSystemObjects()
    {

    }

    /// <summary>
    /// Safe children implementations, returns the children of the item.
    /// </summary>
    public override IEnumerable<BestFileTreeItemViewModel> SafeChildren
    {
        get => _sourceFolder.Children;
    }
}
