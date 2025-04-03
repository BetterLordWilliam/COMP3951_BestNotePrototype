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
    /// Tests file manager renaming functions.
    /// </summary>
    partial class FileManagerTests
    {
        /// <summary>
        /// Tests renaming a file cirrectly.
        /// </summary>

        [TestMethod]
        public void RenameFile_ValidFileAndName_RenamesCorrectly()
        {
            string oldPath = Path.Combine(_testBestNoteDirPath, "old_name.md");
            string expectedNewPath = Path.Combine(_testBestNoteDirPath, "new_name.md");
            File.WriteAllText(oldPath, "content");
            var fileInfo = new FileInfo(oldPath);

            var renamedInfo = _fileSystemService.RenameFile("new_name", fileInfo);

            Assert.IsNotNull(renamedInfo);
            Assert.AreEqual(expectedNewPath, renamedInfo.FullName);
            Assert.IsFalse(File.Exists(oldPath), "Old file should be gone.");
            Assert.IsTrue(File.Exists(expectedNewPath), "New file should exist.");
        }

        /// <summary>
        /// Tests renaming a non-existent target folder.
        /// </summary>
        [TestMethod]
        public void RenameFile_TargetNameExists_ThrowsIOException()
        {
            string path1 = Path.Combine(_testBestNoteDirPath, "file1.md");
            string path2 = Path.Combine(_testBestNoteDirPath, "file2.md");
            File.WriteAllText(path1, "content1");
            File.WriteAllText(path2, "content2");
            var fileToRename = new FileInfo(path1);

            Assert.ThrowsException<IOException>(() =>
                _fileSystemService.RenameFile("file2", fileToRename)
            );
        }

        /// <summary>
        /// Tests renaming a non-existent source folder.
        /// </summary>
        [TestMethod]
        public void RenameFile_NonExistentSource_ThrowsFileNotFoundException()
        {
            var nonExistentFile = new FileInfo(Path.Combine(_testBestNoteDirPath, "ghost.txt"));
            Assert.ThrowsException<FileNotFoundException>(() =>
               _fileSystemService.RenameFile("new_ghost", nonExistentFile)
            );
        }

        /// <summary>
        /// Test renaming a folder correctly.
        /// </summary>
        [TestMethod]
        public void RenameFolder_ValidFolderAndName_RenamesCorrectly()
        {
            string oldPath = Path.Combine(_testBestNoteDirPath, "OldFolder");
            string expectedNewPath = Path.Combine(_testBestNoteDirPath, "NewFolder\\");

            var dirInfo = Directory.CreateDirectory(oldPath);
            var renamedInfo = _fileSystemService.RenameFolder("NewFolder", dirInfo);

            Assert.IsNotNull(renamedInfo);
            Assert.AreEqual(expectedNewPath, renamedInfo.FullName);
            Assert.IsFalse(Directory.Exists(oldPath), "Old folder should be gone.");
            Assert.IsTrue(Directory.Exists(expectedNewPath), "New folder should exist.");
        }
        
        /// <summary>
        /// Test renaming the target when it already exists.
        /// </summary>
        [TestMethod]
        public void RenameFolder_TargetNameExists_ThrowsIOException()
        {
            string path1 = Path.Combine(_testBestNoteDirPath, "folder1");
            string path2 = Path.Combine(_testBestNoteDirPath, "folder2");
            Directory.CreateDirectory(path1);
            Directory.CreateDirectory(path2);
            var dirToRename = new DirectoryInfo(path1);

            Assert.ThrowsException<IOException>(() =>
                _fileSystemService.RenameFolder("folder2", dirToRename)
            );
        }
    }
}
