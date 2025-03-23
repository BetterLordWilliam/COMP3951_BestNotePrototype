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
    /// <param name=""></param>
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
}
