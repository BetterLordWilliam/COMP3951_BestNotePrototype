using BestNote_3951.Services;
using System.Diagnostics;

///
/// Will Otterbein
/// April 2 2025
/// 
namespace BestNote_3951_Tests.FileManagerTests
{
    /// <summary>
    /// Tests methods in the FileManagerService class.
    /// </summary>
    [TestClass]
    public sealed partial class FileManagerTests
    {
        private FileManagerService _fileSystemService;
        private string _testBaseDir;
        private string _currentTestDir;
        private string _testBestNoteDirPath;
        private string _testResourceDirPath;
        private string _testSourceDirPath;

        // Public property for MSTest to inject context
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            // Create unique base and test-specific directories
            _testBaseDir = Path.Combine(Path.GetTempPath(), "FSServiceTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_testBaseDir);

            _currentTestDir = Path.Combine(_testBaseDir, TestContext.TestName ?? Guid.NewGuid().ToString());
            Directory.CreateDirectory(_currentTestDir);

            // Setup standard directories within the test context
            _testBestNoteDirPath = Path.Combine(_currentTestDir, "BestNote");
            _testResourceDirPath = Path.Combine(_currentTestDir, "Resources");
            _testSourceDirPath = Path.Combine(_currentTestDir, "SourceData"); // For move/copy tests
            Directory.CreateDirectory(_testBestNoteDirPath);
            Directory.CreateDirectory(_testResourceDirPath);
            Directory.CreateDirectory(_testSourceDirPath);


            _fileSystemService = new FileManagerService
            {
                // Point service to use test directories
                BestNoteDirectory = new DirectoryInfo(_testBestNoteDirPath),
                ResourceDirectory = new DirectoryInfo(_testResourceDirPath)
            };

            Debug.WriteLine($"[{TestContext.TestName}] Setup complete in: {_currentTestDir}");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                if (Directory.Exists(_testBaseDir))
                {
                    Directory.Delete(_testBaseDir, true);
                    Debug.WriteLine($"[{TestContext?.TestName ?? "Unknown"}] Cleaned up base dir: {_testBaseDir}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during test cleanup for {_testBaseDir ?? "NULL"}: {ex.Message}");
                Console.WriteLine($"WARNING: Test cleanup failed for {_testBaseDir}. Manual cleanup may be needed.");
            }
        }
    }
}

