namespace BestNote_3951_Tests;
using BestNote_3951.ViewModels;
using BestNote_3951.Models;

/// Author: Olivia Grace

/// <summary>
/// Test class for EmbeddedPdfViewModel.cs
/// </summary>
[TestClass]
public class PDFViewModelTest
{
    /// <summary>
    /// An EmbeddedPdfViewModel instance.
    /// </summary>
    EmbeddedPdfViewModel pdfViewModel;

    /// <summary>
    /// Initializes pdfViewModel to be a new EmbeddedPdfViewModel before each test.
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
        pdfViewModel = new EmbeddedPdfViewModel(new BestNote_3951.Services.FileManagerService());
    }

    /// <summary>
    /// Tests that the PdfPath is set to an empty string when an EmbeddedPdfiewModel is initialized.
    /// </summary>
    [TestMethod]
    public void PDFPath_Initial()
    {
        Assert.AreEqual(pdfViewModel.PdfPath, "");
    }

    /// <summary>
    /// Tests the set method of the EmbeddedPdfViewModel PdfPath property.
    /// </summary>
    [TestMethod]
    public void PDFPath_Set()
    {
        String testPath = "testPath";
        pdfViewModel.PdfPath = testPath;
        Assert.AreEqual(pdfViewModel.PdfPath, testPath);
    }

    /// <summary>
    /// Tests that the PdfName is set to an empty string when an EmbeddedPdfiewModel is initialized.
    /// </summary>
    [TestMethod]
    public void PDFName_Initial()
    {
        Assert.AreEqual(pdfViewModel.PdfName, "");
    }

    /// <summary>
    /// Tests the set method of the EmbeddedPdfViewModel PdfName property.
    /// </summary>
    [TestMethod]
    public void PDFName_Set()
    {
        String testName = "testName";
        pdfViewModel.PdfName = testName;
        Assert.AreEqual(pdfViewModel.PdfName, testName);
    }

    /// <summary>
    /// Tests that the PageNum is set to 0 when an EmbeddedPdfiewModel is initialized.
    /// </summary>
    [TestMethod]
    public void PDFPageNum_Initial()
    {
        Assert.AreEqual(pdfViewModel.PageNum, 0);
    }

    /// <summary>
    /// Tests the set method of the EmbeddedPdfViewModel PageNum property.
    /// </summary>
    [TestMethod]
    public void PDFPageNum_Set()
    {
        int testNum = 2;
        pdfViewModel.PageNum = testNum;
        Assert.AreEqual(testNum, pdfViewModel.PageNum);
    }

    /// <summary>
    /// Tests that the OpenPdfFromPath method sets the PdfPath of the EmbeddedPdfViewModel.
    /// </summary>
    [TestMethod]
    public void OpenPdfFromPath_SetPath()
    {
        string testPath = "test";
        pdfViewModel.OpenPDFFromPath(testPath);
        Assert.AreEqual(testPath, pdfViewModel.PdfPath);
    }

}
