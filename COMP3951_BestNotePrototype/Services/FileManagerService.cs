using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Models;

///
/// Will Otterbein
/// March 12 2025
/// 
namespace BestNote_3951.Services;

///
/// Will Otterbein
/// March 19 2025
///
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
    /// Returns the contents of a specified directory, else it returns the root contents.
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public IEnumerable<FileSystemInfo> GetDirectoryInfoContents(string folderName = "", string? parentPath = null)
    {
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, folderName);

        if (!Directory.Exists(combinedPath))
        {
            return new List<FileSystemInfo>();
        }

        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(combinedPath);
            List<FileSystemInfo> contents = new List<FileSystemInfo>();

            contents.AddRange(directoryInfo.GetDirectories());
            contents.AddRange(directoryInfo.GetFiles());

            return contents;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting directory contents: {ex.Message}");

            return Enumerable.Empty<FileSystemInfo>();
        }
    }

    /// <summary>
    /// Creates a new directory.
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public DirectoryInfo? CreateDirectory(string folderName, string? parentPath = null)
    {
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, folderName);

        try
        {
            return Directory.CreateDirectory(combinedPath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating directory: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Creates a new file.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public FileInfo? CreateFile(string fileName, string? parentPath = null)
    {
        string parent = parentPath ?? BestNoteDirectory.FullName;
        string combinedPath = Path.Combine(parent, fileName);

        try
        {
            using (File.Create(combinedPath)) { } // Create empty file.
            return new FileInfo(combinedPath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating file: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Reads the contents of a file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string ReadFileContents(string filePath)
    {
        //try
        //{
        //    return File.ReadAllText(filePath);
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error reading file: {ex.Message}");
        //    return string.Empty;
        //}
        return "";
    }

    /// <summary>
    /// Writes to a file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    public void WriteFileContents(string filePath, string content)
    {
        //try
        //{
        //    File.WriteAllText(filePath, content);
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error writing file: {ex.Message}");
        //}
    }
}

