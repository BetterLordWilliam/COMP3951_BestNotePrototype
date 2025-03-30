using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Models;
using BitMiracle.LibTiff.Classic;
using System.Text.RegularExpressions;

///
/// Will Otterbein
/// March 12 2025
/// 
/// Olivia Grace - Added code to create Resources directory and AddResourceFile method
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
    public DirectoryInfo ResourceDirectory { get; private set; }    // Resource Directory

    private readonly string _appDirectoryPath;
    private readonly string _bestNoteDirectoryPath;
    private readonly string _resourceDirectoryPath;

    /// <summary>
    /// Service constructor. Use with dependency injection.
    /// </summary>
    public FileManagerService()
    {
        _appDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BestNote");
        _bestNoteDirectoryPath = Path.Combine(_appDirectoryPath, "Notes");
        _resourceDirectoryPath = Path.Combine(_appDirectoryPath, "Resources");

        AppDirectory = CreateDirectoryIfNotExists(_appDirectoryPath);
        BestNoteDirectory = CreateDirectoryIfNotExists(_bestNoteDirectoryPath);
        ResourceDirectory = CreateDirectoryIfNotExists(_resourceDirectoryPath);
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

    /// <summary>
    /// Copies a file from a specified file path to the BestNote Resources folder.
    /// </summary>
    /// <param name="fileName">the name of the file to copy</param>
    /// <param name="filePath">the path of the file to copy</param>
    /// <returns>string, the path of where the file was copied to</returns>
    public string AddResourceFile(string fileName, string filePath)
    {
        Debug.WriteLine(fileName);
        // remove special characters from files that are saved to the resource folder
        // because the webview treats it like a link fragment and drops everything after it
        // there might be a better fix but this works
        string name = Regex.Replace(fileName, @"[^\w\s\.\-]", "");
        string parent = ResourceDirectory.FullName;
        string newPath = Path.Combine(parent, name);


        if (!File.Exists(newPath))
        {
            File.Copy(filePath, newPath);
        }

        return newPath;


    }
}

