using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestNote_3951.ViewModels;
using Microsoft.Maui.Controls;

namespace BestNote_3951_Tests.MarkdownEditorTests
{
    [TestClass]
    public class MarkdownRendererViewModelTests
    {
        [TestMethod]
        public void SetsDefeaultWebView()
        {
            // Arrange and Act
            var vm = new MarkdownRendererViewModel();

            // Assert
            Assert.IsNotNull(vm.WebViewSource);
            StringAssert.Contains(
                vm.WebViewSource.Html,
                "<html>",
                "Expected default HTML to contain <html> tag"
                );
        }

        [TestMethod]
        public void SettingWebViewSource_UpdatesProperty()
        {
            // Arrange
            var vm = new MarkdownRendererViewModel();
            var newSource = new HtmlWebViewSource
            {
                Html = "<html><body><h1>Test</h1></body></html>"
            };

            // Act
            vm.WebViewSource = newSource;

            // Assert
            Assert.AreSame(newSource, vm.WebViewSource,
                "Expected the ViewModel to store the new WebViewSource.");
        }
    }
}
