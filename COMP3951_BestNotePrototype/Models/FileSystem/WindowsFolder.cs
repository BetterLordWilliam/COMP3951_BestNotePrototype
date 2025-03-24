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
}

