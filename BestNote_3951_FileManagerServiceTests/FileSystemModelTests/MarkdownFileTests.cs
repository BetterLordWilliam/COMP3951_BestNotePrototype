using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using BestNote_3951.Services;
using BestNote_3951.Models.FileSystem;

namespace BestNote_3951_Tests;

[TestClass]
public class MarkdownFileTests
{
    private FileManagerService _fileSystemService;
    private string _testBestNoteDirPath;
    private string _testResourceDirPath;
    private string _testBaseDir;

    public TestContext TestContext { get; set; }

    /// <summary>
    /// Initialize a file info object for testing.
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        // Create unique base and test-specific directories
        _testBaseDir = Path.Combine(Path.GetTempPath(), "FSServiceTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_testBaseDir);

        _testBestNoteDirPath = Path.Combine(_testBaseDir, "BestNote");
        _testResourceDirPath = Path.Combine(_testBaseDir, "Resources");
        Directory.CreateDirectory(_testBestNoteDirPath);
        Directory.CreateDirectory(_testResourceDirPath);

        _fileSystemService = new FileManagerService
        {
            BestNoteDirectory = new DirectoryInfo(_testBestNoteDirPath),
            ResourceDirectory = new DirectoryInfo(_testResourceDirPath)
        };


        Debug.WriteLine($"[{TestContext.TestName}] Setup complete in: {_testBaseDir}");
    }

    [TestCleanup]
    public void TestCleaup()
    {

    }

    /// <summary>
    /// Test writing the entire contents of a file to the file.
    /// </summary>
    [TestMethod]
    public void WriteFileContents_Success()
    {
        FileInfo? testingFile = _fileSystemService.CreateFile("WillNoteFile");

        Assert.IsNotNull(testingFile);

        MarkdownFile testFile = new MarkdownFile(testingFile, _fileSystemService);
        testFile.WriteToFile("#WillMarkdown\nThis is a markdown string\nThis file is going to be so good!\n");

        Assert.IsTrue(true);
    }

    /// <summary>
    /// Test reading the entire contents of a file and the return.
    /// </summary>
    [TestMethod]
    public void ReadFileContents_Success()
    {
        FileInfo? testingFile = _fileSystemService.CreateFile("WillNoteFileAgain");
        Assert.IsNotNull(testingFile);
        MarkdownFile testFile = new MarkdownFile(testingFile, _fileSystemService);

        string sampleMarkdown = "#WillMarkdown\nThis is a markdown string\nThis file is going to be so good!\n\n##This is mad markdown.";
        testFile.WriteToFile(sampleMarkdown);
        string readValue = testFile.ReadFileContents();

        Assert.AreEqual(sampleMarkdown, readValue);
    }
}
