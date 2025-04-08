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
    /// Tests for creating files.
    /// </summary>
    partial class FileManagerTests
    {

        [TestMethod]
        public void CreateDirectory_DefaultNameInDefaultPath_CreatesDirectory()
        {
            var createdDir = _fileSystemService.CreateDirectory();
            string expectedPath = Path.Combine(_testBestNoteDirPath, "New Folder");

            Assert.IsNotNull(createdDir);
            Assert.AreEqual(expectedPath, createdDir.FullName);
            Assert.IsTrue(Directory.Exists(expectedPath));
        }

        [TestMethod]
        public void CreateDirectory_SpecificNameAndPath_CreatesDirectory()
        {
            string targetPath = Path.Combine(_currentTestDir, "Target");
            // Service method CreateDirectory now ensures parent exists
            // Directory.CreateDirectory(targetPath);

            var createdDir = _fileSystemService.CreateDirectory("MyFolder", targetPath);
            string expectedPath = Path.Combine(targetPath, "MyFolder");

            Assert.IsNotNull(createdDir);
            Assert.AreEqual(expectedPath, createdDir.FullName);
            Assert.IsTrue(Directory.Exists(expectedPath));
        }

        [TestMethod]
        public void CreateDirectory_NameCollision_CreatesUniqueNumberedDirectory()
        {
            Directory.CreateDirectory(Path.Combine(_testBestNoteDirPath, "Collision"));

            var createdDir1 = _fileSystemService.CreateDirectory("Collision"); // Should become Collision (1)
            var createdDir2 = _fileSystemService.CreateDirectory("Collision"); // Should become Collision (2)

            string expectedPath1 = Path.Combine(_testBestNoteDirPath, "Collision (1)");
            string expectedPath2 = Path.Combine(_testBestNoteDirPath, "Collision (2)");

            Assert.AreEqual(expectedPath1, createdDir1.FullName);
            Assert.IsTrue(Directory.Exists(expectedPath1));
            Assert.AreEqual(expectedPath2, createdDir2.FullName);
            Assert.IsTrue(Directory.Exists(expectedPath2));
        }

        [TestMethod]
        public void CreateDirectory_InNonExistentPath_CreatesParentAndDirectory()
        {
            string targetParentPath = Path.Combine(_currentTestDir, "NonExistentParent");
            string folderName = "MyFolder";
            string expectedPath = Path.Combine(targetParentPath, folderName);

            DirectoryInfo createdDir = _fileSystemService.CreateDirectory(folderName, targetParentPath);

            Assert.IsNotNull(createdDir, "Returned DirectoryInfo should not be null.");
            Assert.IsTrue(Directory.Exists(targetParentPath), "Parent directory should have been created.");
            Assert.IsTrue(Directory.Exists(expectedPath), "Target directory should exist on disk.");
            Assert.AreEqual(expectedPath, createdDir.FullName, "DirectoryInfo path should match expected path.");
        }
    }
}
