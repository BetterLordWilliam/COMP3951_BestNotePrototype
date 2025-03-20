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

    public WindowsFolder(DirectoryInfo directoryInfo, FileManagerService fileManagerService)
    {
        _fileManagerService = fileManagerService;
        this.directoryInfo = directoryInfo;
    }

    public DirectoryInfo DirectoryInfo
    {
        get => directoryInfo;
        set => directoryInfo = value;
    }

    public ObservableCollection<ITreeViewItem> Children => throw new NotImplementedException();
}

