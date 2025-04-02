using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// Will otterbein
/// April 2 2025
/// 
namespace BestNote_3951_Tests.FileManagerTests
{
    /// <summary>
    /// Tests methods for adding files in the file system.
    /// </summary>
    partial class FileManagerTests
    {

        [TestMethod]
        public void CreateFile_DefaultNameInDefaultPath_CreatesFileWithMdExtension()
        {
            var createdFile = _fileSystemService.CreateFile();
            string expectedPath = Path.Combine(_testBestNoteDirPath, "New File.md");

            Assert.IsNotNull(createdFile);
            Assert.AreEqual(expectedPath, createdFile.FullName);
            Assert.IsTrue(File.Exists(expectedPath));
            Assert.AreEqual(".md", createdFile.Extension);
        }

        [TestMethod]
        public void CreateFile_SpecificNameNoExtension_AddsMdExtension()
        {
            var createdFile = _fileSystemService.CreateFile("MyDoc", _testBestNoteDirPath);
            string expectedPath = Path.Combine(_testBestNoteDirPath, "MyDoc.md");

            Assert.IsNotNull(createdFile);
            Assert.AreEqual(expectedPath, createdFile.FullName);
            Assert.IsTrue(File.Exists(expectedPath));
        }

        [TestMethod]
        public void CreateFile_SpecificNameWithOtherExtension_ForcesMdExtension()
        {
            var createdFile = _fileSystemService.CreateFile("MyDoc.txt", _testBestNoteDirPath);
            string expectedPath = Path.Combine(_testBestNoteDirPath, "MyDoc.md");

            Assert.IsNotNull(createdFile);
            Assert.AreEqual(expectedPath, createdFile.FullName);
            Assert.IsTrue(File.Exists(expectedPath));
        }

        [TestMethod]
        public void CreateFile_SpecificNameWithMdExtension_UsesProvidedName()
        {
            var createdFile = _fileSystemService.CreateFile("MyDoc.Md", _testBestNoteDirPath);
            string expectedPath = Path.Combine(_testBestNoteDirPath, "MyDoc.Md");

            Assert.IsNotNull(createdFile);
            Assert.AreEqual(expectedPath, createdFile.FullName);
            Assert.IsTrue(File.Exists(expectedPath));
        }

        [TestMethod]
        public void CreateFile_NameCollision_CreatesUniqueNumberedFile()
        {
            File.WriteAllText(Path.Combine(_testBestNoteDirPath, "Doc.md"), "v1");

            var createdFile1 = _fileSystemService.CreateFile("Doc");       // Should be Doc (1).md
            var createdFile2 = _fileSystemService.CreateFile("Doc.md");    // Should be Doc (2).md

            string expectedPath1 = Path.Combine(_testBestNoteDirPath, "Doc (1).md");
            string expectedPath2 = Path.Combine(_testBestNoteDirPath, "Doc (2).md");

            Assert.AreEqual(expectedPath1, createdFile1.FullName);
            Assert.IsTrue(File.Exists(expectedPath1));
            Assert.AreEqual(expectedPath2, createdFile2.FullName);
            Assert.IsTrue(File.Exists(expectedPath2));
        }
    }
}
