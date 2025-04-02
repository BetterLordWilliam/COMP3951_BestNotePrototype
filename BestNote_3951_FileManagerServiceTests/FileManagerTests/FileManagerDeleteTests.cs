using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will Otterbein
/// April 2 2025
/// 
namespace BestNote_3951_Tests.FileManagerTests
{
    /// <summary>
    /// Tests deleting files and folders.
    /// </summary>
    partial class FileManagerTests
    {
        /// <summary>
        /// Tests deleting a file successfully.
        /// </summary>
        [TestMethod]
        public void DeleteFile_ExistingFile_DeletesSuccessfully()
        {
            string filePath = Path.Combine(_testBestNoteDirPath, "delete_me.txt");
            File.WriteAllText(filePath, "content");
            var fileInfo = new FileInfo(filePath);

            _fileSystemService.DeleteFile(fileInfo);

            Assert.IsFalse(File.Exists(filePath), "File should no longer exist.");
        }

        /// <summary>
        /// Tests deleting a file that does not exist.
        /// </summary>
        [TestMethod]
        public void DeleteFile_NonExistentFile_ThrowsFileNotFoundException()
        {
            var nonExistentFile = new FileInfo(Path.Combine(_testBestNoteDirPath, "ghost.txt"));
            Assert.ThrowsException<FileNotFoundException>(() =>
               _fileSystemService.DeleteFile(nonExistentFile)
            );
        }

        /// <summary>
        /// Tests deleting an empty folder non recursively.
        /// </summary>
        [TestMethod]
        public void DeleteFolder_EmptyFolderRecursiveFalse_DeletesSuccessfully()
        {
            string folderPath = Path.Combine(_testBestNoteDirPath, "EmptyToDelete");
            Directory.CreateDirectory(folderPath);
            var dirInfo = new DirectoryInfo(folderPath);

            _fileSystemService.DeleteFolder(dirInfo, recursive: false);

            Assert.IsFalse(Directory.Exists(folderPath), "Directory should no longer exist.");
        }

        /// <summary>
        /// Tests deleting a folder with children non-recursively.
        /// </summary>
        [TestMethod]
        public void DeleteFolder_NonEmptyFolderRecursiveFalse_ThrowsIOException()
        {
            string folderPath = Path.Combine(_testBestNoteDirPath, "NotEmpty");
            Directory.CreateDirectory(folderPath);
            File.WriteAllText(Path.Combine(folderPath, "file.txt"), "content");
            var dirInfo = new DirectoryInfo(folderPath);

            Assert.ThrowsException<IOException>(() =>
               _fileSystemService.DeleteFolder(dirInfo, recursive: false)
            );
            Assert.IsTrue(Directory.Exists(folderPath));
        }

        /// <summary>
        /// Tests recursively deleting a folder and its contents.
        /// </summary>
        [TestMethod]
        public void DeleteFolder_NonEmptyFolderRecursiveTrue_DeletesSuccessfully()
        {
            string folderPath = Path.Combine(_testBestNoteDirPath, "NotEmptyRecursive");
            Directory.CreateDirectory(folderPath);
            File.WriteAllText(Path.Combine(folderPath, "file.txt"), "content");
            Directory.CreateDirectory(Path.Combine(folderPath, "sub"));
            var dirInfo = new DirectoryInfo(folderPath);

            _fileSystemService.DeleteFolder(dirInfo, recursive: true);

            Assert.IsFalse(Directory.Exists(folderPath), "Directory should no longer exist.");
        }

        /// <summary>
        /// Tests deleting a non existent folder.
        /// </summary>
        [TestMethod]
        public void DeleteFolder_NonExistentFolder_ThrowsDirectoryNotFoundException()
        {
            var nonExistentDir = new DirectoryInfo(Path.Combine(_testBestNoteDirPath, "GhostFolder"));
            Assert.ThrowsException<DirectoryNotFoundException>(() =>
              _fileSystemService.DeleteFolder(nonExistentDir)
           );
        }
    }
}
