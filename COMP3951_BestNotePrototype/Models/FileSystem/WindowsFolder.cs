using BestNote_3951.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951.Models.FileSystem;

public class WindowsFolder : IBNFolder
{
    private readonly FileManagerService _fileManagerService;
    private DirectoryInfo directoryInfo;
    private ObservableCollection<BestFileTreeItemViewModel> children;

    /// <summary>
    /// Initializes a new WindowsFolder instance.
    /// </summary>
    /// <param name="directoryInfo"></param>
    /// <param name="fileManagerService"></param>
    public WindowsFolder(
        DirectoryInfo       directoryInfo,
        FileManagerService  fileManagerService
    ) {
        _fileManagerService = fileManagerService;
        this.children = new ObservableCollection<BestFileTreeItemViewModel>();
        this.directoryInfo = directoryInfo;
    }

    public DirectoryInfo DirectoryInfo
    {
        get => directoryInfo;
        set => directoryInfo = value;
    }

    public ObservableCollection<BestFileTreeItemViewModel> Children
    {
        get => children;
        set => children = value;
    }

    /// <summary>
    /// Renames a windows folder object.
    /// </summary>
    /// <param name="NewItemName"></param>
    public void Rename(string NewItemName)
    {
        DirectoryInfo UpdatedDirectoryInfo = _fileManagerService.RenameFolder(NewItemName, DirectoryInfo);
        DirectoryInfo = UpdatedDirectoryInfo;
    }

    /// <summary>
    /// Adds a new child to folders collection of children.
    /// </summary>
    /// <param name="NewChild"></param>
    public void AddChild(BestFileTreeItemViewModel NewChild)
    {
        children.Add(NewChild);
    }

    /// <summary>
    /// Removes a child node from the nodes collection of children.
    /// </summary>
    /// <param name="TargetChild"></param>
    public void RemoveChild(BestFileTreeItemViewModel TargetChild)
    {
        bool status = children.Remove(TargetChild);
        Debug.WriteLine(status);
    }

    /// <summary>
    /// Move this item to the new item location in the file systme.
    /// </summary>
    /// <param name="NewParent"></param>
    public void Move(FolderTreeItem NewParent)
    {
        DirectoryInfo NewDirInfo = _fileManagerService.MoveFolder(DirectoryInfo, NewParent.DirectoryInfo);
        DirectoryInfo = NewDirInfo;
    }

    /// <summary>
    /// Deletes a windows folder from the file system.
    /// </summary>
    public void Delete()
    {
        _fileManagerService.DeleteFolder(DirectoryInfo);
    }

    /// <summary>
    /// Deletes a windows folder form the file system and it's sub items.
    /// </summary>
    public void DeleteAll()
    {
        _fileManagerService.DeleteFolder(DirectoryInfo, true);
    }

    /// <summary>
    /// Loads the contents of the file system of this folder.
    /// </summary>
    public void LoadFileSystemObjects()
    {

    }
}

