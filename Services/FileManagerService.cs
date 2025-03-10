using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Models;

namespace BestNote_3951.Services
{
    public class FileManagerService
    {
        public DirectoryInfo AppDirectory;                // Application data directory

        public DirectoryInfo BestNoteDirectory;           // Notes directory

        /// <summary>
        /// Service constructor. Use with dependency injection.
        /// </summary>
        public FileManagerService()
        {
            // Define the directory, possible configureable, for now ..\OS_Data\BestNote\...
            string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BestNote");
            // Define the notes directory, possible configureable, for now notes
            string bestNoteDirectory = Path.Combine(appDirectory, "Notes");

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
        /// Returns the contents of BestNote root directory.
        /// </summary>
        /// <returns></returns>
        public DirectoryInfo GetRootContents()
        {
            // Testing output
            return AppDirectory;
        }

        /// <summary>
        /// Returns the contents of the Bestnote Notes directory.
        /// </summary>
        /// <returns></returns>
        public DirectoryInfo GetNotesContents()
        {
            // Testing output
            return BestNoteDirectory;
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
                return new BestFile(folderName, "folder_icon.png", directoryInfo, parentDirectoryInfo);
            }
            directoryInfo = Directory.CreateDirectory(combinedPath);

            return new BestFile(folderName, "folder_icon.png", directoryInfo, parentDirectoryInfo);
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
            DirectoryInfo directoryInfo = new DirectoryInfo(combinedPath);
            DirectoryInfo parentDirectoryInfo = new DirectoryInfo(parent);

            return new BestFile(fileName, "md_file.png", directoryInfo, parentDirectoryInfo);
        }
    }
}
