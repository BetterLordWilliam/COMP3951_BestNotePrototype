using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951_Tests.FileManagerTests
{
    /// <summary>
    /// Class tests the get contents methods of the file manager system.
    /// </summary>
    partial class FileManagerTests
    {
        /// <summary>
        /// Tests getting the contents of a valid directory.
        /// </summary>
        [TestMethod]
        public void GetDirectoryInfoContents_ValidPathWithItems_ReturnsItems()
        {
            string subDir = Path.Combine(_testBestNoteDirPath, "Sub");
            string file1 = Path.Combine(_testBestNoteDirPath, "f1.txt");
            string fileInSub = Path.Combine(subDir, "subfile.txt");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(file1, "");
            File.WriteAllText(fileInSub, "");

            var contents = _fileSystemService.GetDirectoryInfoContents(_testBestNoteDirPath).ToList();

            Assert.AreEqual(2, contents?.Count);
            Assert.IsTrue(contents.Any(fsi => fsi.Name == "Sub" && fsi is DirectoryInfo));
            Assert.IsTrue(contents.Any(fsi => fsi.Name == "f1.txt" && fsi is FileInfo));
        }

        /// <summary>
        /// Tests gettign the contents of an empty directroy
        /// </summary>
        [TestMethod]
        public void GetDirectoryInfoContents_EmptyDirectory_ReturnsEmptyList()
        {
            var contents = _fileSystemService.GetDirectoryInfoContents(_testBestNoteDirPath).ToList();
            Assert.AreEqual(0, contents?.Count);
        }

        /// <summary>
        /// Tests getting the contents of a bad path.
        /// </summary>
        [TestMethod]
        public void GetDirectoryInfoContents_NonExistentPath_ThrowsDirectoryNotFoundException()
        {
            string badPath = Path.Combine(_currentTestDir, "BadPath");
            Assert.ThrowsException<DirectoryNotFoundException>(() =>
                _fileSystemService.GetDirectoryInfoContents(badPath)
            );
        }

        /// <summary>
        /// Tests getting the contents of a null path (base path used instead).
        /// </summary>
        [TestMethod]
        public void GetDirectoryInfoContents_NullPathUsesBestNote_ReturnsBestNoteContents()
        {
            string fileInBestNote = Path.Combine(_testBestNoteDirPath, "best_note_file.txt");
            File.WriteAllText(fileInBestNote, "");

            var contents = _fileSystemService.GetDirectoryInfoContents(null).ToList();

            Assert.AreEqual(1, contents?.Count);
            Assert.AreEqual("best_note_file.txt", contents[0].Name);
        }
    }
}
