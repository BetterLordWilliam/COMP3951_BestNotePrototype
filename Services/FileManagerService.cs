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

    private string appDirectory;
    private string bestNoteDirectory;

    /// <summary>
    /// Service constructor. Use with dependency injection.
    /// </summary>
    public FileManagerService()
    {
        // Define the directory, possible configureable, for now ..\OS_Data\BestNote\...
        appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BestNote");
        // Define the notes directory, possible configureable, for now notes
        bestNoteDirectory = Path.Combine(appDirectory, "Notes");

        // Create directory information objects
        if (!Directory.Exists(appDirectory))
        {
            Directory.CreateDirectory(appDirectory);
        }
        AppDirectory = new DirectoryInfo(appDirectory);

        // Create directory information objects
        if (!Directory.Exists(bestNoteDirectory))
        {
            Directory.CreateDirectory(bestNoteDirectory);
        }
        BestNoteDirectory = new DirectoryInfo(bestNoteDirectory);
    }

    /// <summary>
    /// Get the contents of a directory as an observable collection.
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public ObservableCollection<BestFile>? GetFolderContents(string folderName = "", string? parentPath = null)
    {
        // Determine parent
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, folderName);
        DirectoryInfo directoryInfo;
        ObservableCollection<BestFile> contents = new ObservableCollection<BestFile>();

        // If directory does not exist abort
        if (!Directory.Exists(combinedPath))
            return null;

        directoryInfo = new DirectoryInfo(combinedPath);

        foreach (DirectoryInfo d in directoryInfo.GetDirectories())
        {
            contents.Add(BestFile.BestFileFolder(d.Name, "folder_icon.png", d, directoryInfo));
        }
        foreach (FileInfo d in directoryInfo.GetFiles())
        {
            contents.Add(BestFile.BestFileMarkdown(d.Name, "md_file.png", d, directoryInfo));
        }

        return contents;
    }

    /// <summary>
    /// Create a folder in the file system. Parent folder path is not required.
    /// </summary>
    /// <param name="ParentPath"></param>
    /// <param name="folderName"></param>
    /// <returns></returns>
    public BestFile? AddFolder(string folderName, string? parentPath = null)
    {
        // Determine parent
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, folderName);
        DirectoryInfo directoryInfo, parentDirectoryInfo = new DirectoryInfo(parent);

        // Create directory if it does not exist
        if (Directory.Exists(combinedPath))
        { 
            directoryInfo = new DirectoryInfo(combinedPath);
            return BestFile.BestFileFolder(folderName, "folder_icon.png", directoryInfo, parentDirectoryInfo);
        }
        directoryInfo = Directory.CreateDirectory(combinedPath);

        return BestFile.BestFileFolder(folderName, "folder_icon.png", directoryInfo, parentDirectoryInfo);
    }

    /// <summary>
    /// Create a file in the apps local data. Parent folder and file extension are not required, default is md.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="fileExtension"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public BestFile? AddFile(string fileName, string fileExtension = ".md", string? parentPath = null)
    {
        // Determine fileType and parent
        string fileType = string.Join("", fileName, fileExtension);
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, fileType);

        // If the file exists, try and delete it
        if (File.Exists(combinedPath))
            File.Delete(combinedPath);

        // Create the file
        File.Create(combinedPath);
        FileInfo directoryInfo = new FileInfo(combinedPath);
        DirectoryInfo parentDirectoryInfo = new DirectoryInfo(parent);

        return BestFile.BestFileMarkdown(fileName, "md_file.png", directoryInfo, parentDirectoryInfo);
    }
}

