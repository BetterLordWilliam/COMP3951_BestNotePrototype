using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951_Tests.FileManagerTests
{
    partial class FileManagerTests
    {
        /// <summary>
        /// Tests moving a file into a target location successfully.
        /// </summary>
        [TestMethod]
        public void MoveFile_ValidFileAndDestination_MovesFile()
        {
            string sourceFilePath = Path.Combine(_testSourceDirPath, "move_me.txt");
            string destDirPath = Path.Combine(_currentTestDir, "Destination");
            string expectedNewPath = Path.Combine(destDirPath, "move_me.txt");

            File.WriteAllText(sourceFilePath, "content");
            Directory.CreateDirectory(destDirPath);

            var fileToMove = new FileInfo(sourceFilePath);
            var destDirInfo = new DirectoryInfo(destDirPath);

            var movedFile = _fileSystemService.MoveFile(fileToMove, destDirInfo);

            Assert.IsNotNull(movedFile);
            Assert.AreEqual(expectedNewPath, movedFile.FullName);
            Assert.IsFalse(File.Exists(sourceFilePath), "Original file should not exist.");
            Assert.IsTrue(File.Exists(expectedNewPath), "Moved file should exist in destination.");
        }

        /// <summary>
        /// Tests moving a file into a target location where a file with the name already exists.
        /// </summary>
        [TestMethod]
        public void MoveFile_TargetNameExistsInDestination_ThrowsIOException()
        {
            string sourceFilePath = Path.Combine(_testSourceDirPath, "collision.txt");
            string destDirPath = Path.Combine(_currentTestDir, "Destination");
            string existingFilePath = Path.Combine(destDirPath, "collision.txt");

            File.WriteAllText(sourceFilePath, "source content");
            Directory.CreateDirectory(destDirPath);
            File.WriteAllText(existingFilePath, "destination content");

            var fileToMove = new FileInfo(sourceFilePath);
            var destDirInfo = new DirectoryInfo(destDirPath);

            Assert.ThrowsException<IOException>(() =>
                _fileSystemService.MoveFile(fileToMove, destDirInfo)
            );
            Assert.IsTrue(File.Exists(sourceFilePath));
        }

        /// <summary>
        /// Tests moving a file into a non existent directory.
        /// </summary>
        [TestMethod]
        public void MoveFile_NonExistentDestinationDir_ThrowsDirectoryNotFoundException()
        {
            string sourceFilePath = Path.Combine(_testSourceDirPath, "move_me.txt");
            string destDirPath = Path.Combine(_currentTestDir, "NonExistentDest");

            File.WriteAllText(sourceFilePath, "content");

            var fileToMove = new FileInfo(sourceFilePath);
            var destDirInfo = new DirectoryInfo(destDirPath);

            Assert.ThrowsException<DirectoryNotFoundException>(() =>
                _fileSystemService.MoveFile(fileToMove, destDirInfo)
            );
        }

        /// <summary>
        /// Test moving a folder into a location where a folder with its name exists.
        /// </summary>
        [TestMethod]
        public void MoveFolder_TargetNameExistsInDestination_ThrowsIOException()
        {
            string sourceFolderPath = Path.Combine(_testSourceDirPath, "CollisionFolder");
            string destDirPath = Path.Combine(_currentTestDir, "Destination");
            string existingFolderPath = Path.Combine(destDirPath, "CollisionFolder");
            Directory.CreateDirectory(sourceFolderPath);
            Directory.CreateDirectory(destDirPath);
            Directory.CreateDirectory(existingFolderPath);
            var folderToMove = new DirectoryInfo(sourceFolderPath);
            var destDirInfo = new DirectoryInfo(destDirPath);

            Assert.ThrowsException<IOException>(() =>
                _fileSystemService.MoveFolder(folderToMove, destDirInfo)
            );
            Assert.IsTrue(Directory.Exists(sourceFolderPath));
        }

        /// <summary>
        /// Test moving a folder into itself.
        /// </summary>
        [TestMethod]
        public void MoveFolder_IntoItself_ThrowsIOException()
        {
            string sourceFolderPath = Path.Combine(_testSourceDirPath, "FolderA");
            Directory.CreateDirectory(sourceFolderPath);
            var folderToMove = new DirectoryInfo(sourceFolderPath);
            var destDirInfo = new DirectoryInfo(sourceFolderPath);

            Assert.ThrowsException<IOException>(() =>
                _fileSystemService.MoveFolder(folderToMove, destDirInfo)
            );
        }
        
        /// <summary>
        /// Test moving a folder into it's own subdirectory.
        /// </summary>
        [TestMethod]
        public void MoveFolder_IntoItsSubdirectory_ThrowsIOException()
        {
            string sourceFolderPath = Path.Combine(_testSourceDirPath, "FolderA");
            string subFolderPath = Path.Combine(sourceFolderPath, "SubFolder");
            Directory.CreateDirectory(sourceFolderPath);
            Directory.CreateDirectory(subFolderPath);
            var folderToMove = new DirectoryInfo(sourceFolderPath);
            var destDirInfo = new DirectoryInfo(subFolderPath);

            Assert.ThrowsException<IOException>(() =>
                _fileSystemService.MoveFolder(folderToMove, destDirInfo)
            );
        }
    }
}
