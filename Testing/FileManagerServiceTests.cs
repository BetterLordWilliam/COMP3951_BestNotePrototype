using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestNote_3951.Services;
using BestNote_3951.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestNote_3951.Testing
{
    [TestClass]
    public sealed class FileSystemServiceTests
    {
        public FileManagerService bs = new FileManagerService();

        /// <summary>
        /// Test for adding a folder to the file system.
        /// </summary>
        [TestMethod]
        public void TestRootDirectory()
        {
            // Get the root contents of the parent file
            DirectoryInfo output = bs.AppDirectory;
            Assert.IsNotNull(output);
            Debug.WriteLine($"Best note app data directory {output.FullName}");
        }

        /// <summary>
        /// Test for validatitng the root directory was created.
        /// </summary>
        [TestMethod]
        public void TestNoteDirectory()
        {
            // Get the root contents of the parent file
            TestRootDirectory();
            DirectoryInfo output = bs.BestNoteDirectory;
            Assert.IsNotNull(output);
            Debug.WriteLine($"Best note notes directory {output.FullName}");
        }

        /// <summary>
        /// Test for adding folders to the file system.
        /// </summary>
        [TestMethod]
        public void TestAddFolder()
        {
            TestNoteDirectory();
            BestFile output = bs.AddFolder("Will Testing Notes");
            Assert.IsNotNull(output);
            Debug.WriteLine($"Best note new folder directory {output.DirectoryInfo.FullName}\nBest note new folder name {output.DirectoryInfo.Name}");
        }

        /// <summary>
        /// Test for adding folders into other folders.
        /// </summary>
        [TestMethod]
        public void TestAddFolderToFolder()
        {
            TestNoteDirectory();
            BestFile output = bs.AddFolder("Will Testing Notes");
            Assert.IsNotNull(output);
            Debug.WriteLine($"Best note test parent {output.DirectoryInfo.FullName}");
            BestFile output1 = bs.AddFolder("Will Testing Notes Sub", output.DirectoryInfo.FullName);
            Assert.IsNotNull(output1);
            Debug.WriteLine($"Best note new folder directory {output1.DirectoryInfo.FullName}\nBest note new folder name {output1.DirectoryInfo.Name}");
        }

        /// <summary>
        /// Test for adding folders to the file system.
        /// </summary>
        [TestMethod]
        public void TestAddFile()
        {
            TestNoteDirectory();
            BestFile output = bs.AddFile("My notes");
            Assert.IsNotNull(output);
            Debug.WriteLine($"Best note new folder directory {output.DirectoryInfo.FullName}\nBest note new folder name {output.DirectoryInfo.Name}");
        }

        /// <summary>
        /// Test for adding folders to the file system.
        /// </summary>
        [TestMethod]
        public void TestAddFileToFolder()
        {
            TestNoteDirectory();
            BestFile output = bs.AddFolder("Will Testing Notes");
            Assert.IsNotNull(output);
            Debug.WriteLine($"Best note test parent {output.DirectoryInfo.FullName}");
            BestFile output1 = bs.AddFile("Will Testing Notes", parentPath: output.DirectoryInfo.FullName);
            Assert.IsNotNull(output1);
            Debug.WriteLine($"Best note new folder directory {output1.DirectoryInfo.FullName}\nBest note new folder name {output1.DirectoryInfo.Name}");
        }
    }

}
