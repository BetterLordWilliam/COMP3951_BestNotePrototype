using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (!Directory.Exists(appDirectory))
            {
                // Create directory information objects
                Directory.CreateDirectory(appDirectory);
            }
            AppDirectory = new DirectoryInfo(appDirectory);

            if (!Directory.Exists(bestNoteDirectory))
            {
                // Create directory information objects
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
        public DirectoryInfo AddFolder(string folderName, string? parentPath = null)
        {
            // Create the directory if is does not exist
            string combinedPath = Path.Combine(parentPath ?? BestNoteDirectory.FullName, folderName);
            if (Directory.Exists(combinedPath))
                return new DirectoryInfo(combinedPath);
            return Directory.CreateDirectory(combinedPath);
        }

        /// <summary>
        /// Create a file in the apps local data. Parent folder and file extension are not required, default is md.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public FileStream AddFile(string fileName, string fileExtension = ".md", string? parentPath = null)
        {
            // Create the directory if is does not exist
            string fileType = string.Join("", fileName, fileExtension);
            string combinedPath = Path.Combine(parentPath ?? BestNoteDirectory.FullName, fileType);

            if (File.Exists(combinedPath))
                File.Delete(combinedPath);

            return File.Create(combinedPath);
        }
    }
}
