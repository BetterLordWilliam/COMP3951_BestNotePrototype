using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestNote_3951.Services;

namespace BestNote_3951_Tests.Services
{
    [TestClass]
    public class TableOfContentBuilderTests
    {
        [TestMethod]
        public void TableOfContentizer_SingleHeading_ReturnsHtmlWithHeadingText()
        {
            // Arrange
            string markdown = "# Hello World";

            // Act
            string html = TableOfContentBuilder.TableOfContentizer(markdown);

            // Assert
            Assert.IsNotNull(html, "Expected non-null HTML output.");
            StringAssert.Contains(html, "<html>", "Output should contain <html>.");
            StringAssert.Contains(html, "Hello World", "Output should contain the heading text.");
        }

        [TestMethod]
        public void TableOfContentizer_SubHeading_ReturnsHtmlWithSubHeadingText()
        {
            // Arrange
            string markdown = "## Subheading";

            // Act
            string html = TableOfContentBuilder.TableOfContentizer(markdown);

            // Assert
            Assert.IsNotNull(html, "Expected non-null HTML output.");
            StringAssert.Contains(html, "<html>", "Output should contain <html>.");
            StringAssert.Contains(html, "Subheading", "Output should contain the heading text.");
        }
    }
}
