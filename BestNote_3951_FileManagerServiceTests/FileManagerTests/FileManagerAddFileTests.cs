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
    }
}
