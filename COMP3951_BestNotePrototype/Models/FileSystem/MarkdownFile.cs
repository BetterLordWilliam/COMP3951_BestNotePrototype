using BestNote_3951.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951.Models.FileSystem;

public class MarkdownFile : IBNFile, IBNWritable, IBNReadable
{
    private readonly FileManagerService _fileManagerService;
    private FileInfo fileInfo;

    /// <summary>
    /// Initializes a new MarkDown file instnace.
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <param name="fileManagerService"></param>
    public MarkdownFile(
        FileInfo            fileInfo,
        FileManagerService  fileManagerService
    ) {
        _fileManagerService = fileManagerService;
        this.fileInfo = fileInfo;
    }

    public FileInfo FileInfo
    {
        get => fileInfo;
        set => fileInfo = value;
    }

    /// <summary>
    /// Renames a file in the file system.
    /// </summary>
    /// <param name="NewItemName"></param>
    public void Rename(string NewItemName)
    {
        FileInfo UpdatedFileInfo = _fileManagerService.RenameFile(NewItemName, FileInfo);
        FileInfo = UpdatedFileInfo;
    }

    /// <summary>
    /// Move this file to a new location in the file system.
    /// </summary>
    /// <param name="NewParent"></param>
    public void Move(FolderTreeItem NewParent)
    {

    }

    public string ReadFileContents()
    {
        return "";
    }

    public void WriteToFile(string Content)
    {
        
    }
}
