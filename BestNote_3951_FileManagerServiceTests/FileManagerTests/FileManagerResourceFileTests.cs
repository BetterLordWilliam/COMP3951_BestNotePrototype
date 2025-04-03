using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestNote_3951.Services;

///
/// Will Otterbein
/// April 2 2025
/// 
namespace BestNote_3951_Tests.FileManagerTests
{
    /// <summary>
    /// Resource file tests.
    /// </summary>
    partial class FileManagerTests
    {
        [TestMethod]
        public void AddResourceFile_NewFile_CopiesToResourcesAndReturnsPath()
        {
            string sourceFileName = "my_resource.jpg";
            string sourceFilePath = Path.Combine(_testSourceDirPath, sourceFileName);
            string expectedDestPath = Path.Combine(_testResourceDirPath, sourceFileName);
            File.WriteAllText(sourceFilePath, "image data");

            string resultPath = _fileSystemService.AddResourceFile(sourceFileName, sourceFilePath);

            Assert.AreEqual(expectedDestPath, resultPath);
            Assert.IsTrue(File.Exists(expectedDestPath), "File should exist in resources.");
            Assert.AreEqual("image data", File.ReadAllText(expectedDestPath));
        }

        [TestMethod]
        public void AddResourceFile_FileAlreadyExistsInResources_DoesNotCopyAndReturnsPath()
        {
            string sourceFileName = "existing_resource.png";
            string sourceFilePath = Path.Combine(_testSourceDirPath, sourceFileName);
            string existingDestPath = Path.Combine(_testResourceDirPath, sourceFileName);

            File.WriteAllText(existingDestPath, "existing data");
            DateTime existingWriteTime = File.GetLastWriteTimeUtc(existingDestPath);

            File.WriteAllText(sourceFilePath, "new data");

            string resultPath = _fileSystemService.AddResourceFile(sourceFileName, sourceFilePath);

            Assert.AreEqual(existingDestPath, resultPath);
            Assert.IsTrue(File.Exists(existingDestPath));
            Assert.AreEqual("existing data", File.ReadAllText(existingDestPath), "Content should not change.");
            Assert.AreEqual(existingWriteTime, File.GetLastWriteTimeUtc(existingDestPath), "Write time should not change.");
        }

        [TestMethod]
        public void AddResourceFile_NonExistentSourceFile_ThrowsFileNotFoundException()
        {
            string sourceFileName = "non_existent_source.dat";
            string sourceFilePath = Path.Combine(_testSourceDirPath, sourceFileName);

            Assert.ThrowsException<FileNotFoundException>(() =>
                _fileSystemService.AddResourceFile(sourceFileName, sourceFilePath)
            );
        }

        [TestMethod]
        public void AddResourceFile_NullOrEmptyFileName_ThrowsArgumentException()
        {
            string sourceFilePath = Path.Combine(_testSourceDirPath, "somefile.txt");
            File.WriteAllText(sourceFilePath, "data");

            Assert.ThrowsException<ArgumentException>(() => _fileSystemService.AddResourceFile(null, sourceFilePath));
            Assert.ThrowsException<ArgumentException>(() => _fileSystemService.AddResourceFile("", sourceFilePath));
            Assert.ThrowsException<ArgumentException>(() => _fileSystemService.AddResourceFile(" ", sourceFilePath));
        }
    }
}
