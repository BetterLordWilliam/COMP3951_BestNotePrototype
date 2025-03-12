using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Models;

namespace BestNote_3951.Services;

public class FileManagerService
{
    public DirectoryInfo AppDirectory { get; private set; }         // Application data directory
    public DirectoryInfo BestNoteDirectory { get; private set; }    // Notes directory

    private readonly string _appDirectoryPath;
    private readonly string _bestNoteDirectoryPath;

    /// <summary>
    /// Service constructor. Use with dependency injection.
    /// </summary>
    public FileManagerService()
    {
        _appDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BestNote");
        _bestNoteDirectoryPath = Path.Combine(_appDirectoryPath, "Notes");

        AppDirectory = CreateDirectoryIfNotExists(_appDirectoryPath);
        BestNoteDirectory = CreateDirectoryIfNotExists(_bestNoteDirectoryPath);
    }

    /// <summary>
    /// Create a directory if it does not exist in the system.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private DirectoryInfo CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return new DirectoryInfo(path);
    }

    /// <summary>
    /// Get the contents of a directory as an observable collection.
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public IReadOnlyCollection<BestFile> GetFolderContents(string folderName = "", string? parentPath = null)
    {
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, folderName);

        // Debug.WriteLine($"{parent} {combinedPath}");

        if (!Directory.Exists(combinedPath))
        {
            return new List<BestFile>(); // Return empty list
        }

        DirectoryInfo directoryInfo = new DirectoryInfo(combinedPath);
        List<BestFile> contents = new List<BestFile>();

        foreach (DirectoryInfo d in directoryInfo.GetDirectories())
        {
            contents.Add(BestFile.BestFileFolder(d.Name, "folder_icon.png", d, directoryInfo));
        }

        foreach (FileInfo f in directoryInfo.GetFiles())
        {
            contents.Add(BestFile.BestFileMarkdown(f.Name, "md_file.png", f, directoryInfo));
        }


        return contents;
    }

    /// <summary>
    /// Create a folder in the file system. Parent folder path is not required.
    /// </summary>
    /// <param name="ParentPath"></param>
    /// <param name="folderName"></param>
    /// <returns></returns>
    public BestFile AddFolder(string folderName, string? parentPath = null)
    {
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, folderName);
        DirectoryInfo parentDirectoryInfo = new DirectoryInfo(parent);

        DirectoryInfo directoryInfo = Directory.CreateDirectory(combinedPath);

        return BestFile.BestFileFolder(folderName, "folder_icon.png", directoryInfo, parentDirectoryInfo);
    }

    /// <summary>
    /// Create a file in the apps local data. Parent folder and file extension are not required, default is md.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public BestFile? AddFile(string fileName, string? parentPath = null)
    {
        Debug.WriteLine(fileName);
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, fileName);

        using (File.Create(combinedPath)) { } // Create empty file.
        FileInfo fileInfo = new FileInfo(combinedPath);
        DirectoryInfo parentDirectoryInfo = new DirectoryInfo(parent);

        return BestFile.BestFileMarkdown(fileName, "md_file.png", fileInfo, parentDirectoryInfo);
    }
}

