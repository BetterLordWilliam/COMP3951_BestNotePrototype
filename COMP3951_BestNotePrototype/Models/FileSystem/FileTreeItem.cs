using BestNote_3951.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// March 23 2025
/// 
namespace BestNote_3951.Models.FileSystem;

/// <summary>
/// Tree File Items.
/// </summary>
public partial class FileTreeItem : TreeViewItemBase, IBNFile
{
    private readonly IBNFile _sourceFile;

    /// <summary>
    /// File tree file item class.
    /// </summary>
    /// <param name="sourceFile"></param>
    /// <param name="itemLevel"></param>
    /// <param name="indentationPadding"></param>
    public FileTreeItem(
        int itemLevel,
        Thickness indentationPadding,
        IBNFile sourceFile
    )
    {
        _sourceFile = sourceFile;

        ItemName = sourceFile.FileInfo.Name;
        ImageIcon = "md_file.png";
        ItemLevel = itemLevel;
        IndentationPadding = indentationPadding;
    }

    // Source file delegated implementations
    public FileInfo FileInfo
    {
        get => _sourceFile.FileInfo;
        set => _sourceFile.FileInfo = value;
    }

    /// <summary>
    /// Renames a folder item.
    /// </summary>
    /// <param name="NewItemName"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Rename(string NewItemName)
    {
        _sourceFile.Rename(NewItemName);
        ItemName = _sourceFile.FileInfo.Name;
    }

    /// <summary>
    /// Move a file to it's new location in the file system.
    /// </summary>
    /// <param name="NewParent"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void Move(FolderTreeItem NewParent)
    {
        // Use the file manager service to move the item
        _sourceFile.Move(NewParent);

        // Update the level and padding values according to the move
        ItemLevel = (ItemLevel == 0) ? 0 : NewParent.ItemLevel + 10;
        IndentationPadding = NewParent.IndentationPadding + new Thickness(10, 0, 0, 0);
    }
}
