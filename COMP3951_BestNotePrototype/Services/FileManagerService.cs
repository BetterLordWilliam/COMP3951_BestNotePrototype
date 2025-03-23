using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Models;
using Microsoft.Maui.Storage;

///
/// Will Otterbein
/// March 12 2025
/// 
namespace BestNote_3951.Services
{
    /// <summary>
    /// Thrown if the file system tries to do work on an item that does not exist.
    /// </summary>
    public class FileSystemObjectDoesNotExist : Exception
    {
        /// <summary>
        /// Initializes a file system object with a message string.
        /// </summary>
        /// <param name="Message"></param>
        public FileSystemObjectDoesNotExist(string Message) : base(Message) { }
    }

    /// <summary>
    /// Service for managing application files in the file system.
    /// </summary>
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
        /// <param name="FolderName"></param>
        /// <param name="ParentPath"></param>
        /// <returns></returns>
        public IEnumerable<FileSystemInfo> GetDirectoryInfoContents(string? TargetPath)
        {
            string RealTargetPath = TargetPath ?? BestNoteDirectory.FullName;

            if (!Directory.Exists(RealTargetPath))
            {
                throw new FileSystemObjectDoesNotExist($"Cannot get contents of item at: {RealTargetPath}.\n\n Directory does not exist");
            }
            
            DirectoryInfo directoryInfo = new(RealTargetPath);
            List<FileSystemInfo> contents = new();

            contents.AddRange(directoryInfo.GetDirectories());
            contents.AddRange(directoryInfo.GetFiles());

            return contents;
        }

        /// <summary>
        /// Creates a new directory.
        /// </summary>
        /// <param name="FolderName"></param>
        /// <param name="ParentPath"></param>
        /// <returns></returns>
        public DirectoryInfo? CreateDirectory(string FolderName, string? ParentPath = null)
        {

            string Parent = ParentPath ?? BestNoteDirectory.FullName;
            string CombinedPath = Path.Combine(Parent, FolderName);

            return Directory.CreateDirectory(CombinedPath);
        }

        /// <summary>
        /// Creates a new file.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="TargetPath"></param>
        /// <returns></returns>
        public FileInfo? CreateFile(string FileName, string? TargetPath = null)
        {
            string Parent = TargetPath ?? BestNoteDirectory.FullName;
            string CombinedPath = Path.Combine(Parent, FileName);


            using (File.Create(CombinedPath)) { } // Create empty file.
            return new FileInfo(CombinedPath);
        }

        /// <summary>
        /// Rename an item on the file system.
        /// </summary>
        /// <p>
        /// Try and find the file in the file system. If it exists rename it to the supplied name.
        /// </p>
        /// <param name="newName"></param>
        /// <param name="itemInfo"></param>
        /// <returns></returns>
        public FileInfo RenameFile(string newName, FileInfo itemInfo)
        {
            DirectoryInfo? ParentPath = Directory.GetParent(itemInfo.FullName);
            if (ParentPath is null)
                throw new NullReferenceException($"Parent path of item is null {ParentPath}");

            string newPath = Path.Combine(ParentPath.FullName, newName);
            itemInfo.MoveTo(newPath);
            return itemInfo;
        }

        /// <summary>
        /// Rename a folder in the file system.
        /// </summary>
        /// <param name="newName"></param>
        /// <param name=""></param>
        /// <returns></returns>
        //public DirectoryInfo RenameFolder(string newName, DirectoryInfo)
        //{
        //    return null;
        //}

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
}



