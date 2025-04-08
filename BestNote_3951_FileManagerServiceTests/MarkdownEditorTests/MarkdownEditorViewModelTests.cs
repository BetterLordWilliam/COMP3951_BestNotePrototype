using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestNote_3951.ViewModels;
using BestNote_3951.Services;

namespace BestNote_3951_Tests.MarkdownEditorTests
{
    [TestClass]
    public class MarkdownEditorViewModelBasicTests
    {
        [TestMethod]
        public void Constructor_SetsDefaultMarkdownText()
        {
            // Arrange & Act
            var vm = new MarkdownEditorViewModel(new AlertService());

            // Assert
            Assert.AreEqual("# Hello", vm.MarkdownText);
        }

        [TestMethod]
        public void MarkdownTextProperty_CanBeChanged()
        {
            // Arrange
            var vm = new MarkdownEditorViewModel(new AlertService());
            var expected = "## A new heading";

            // Act
            vm.MarkdownText = expected;

            // Assert
            Assert.AreEqual(expected, vm.MarkdownText);
        }

        [TestMethod]
        public void ChangingMarkdown_PropertyChangedEvent_Test()
        {
            // Arrange
            var vm = new MarkdownEditorViewModel(new AlertService());
            bool raised = false;

            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(vm.MarkdownText))
                {
                    raised = true;
                }
            };

            // Act
            vm.MarkdownText = "Best Test Text yyyyyeeeee";

            // Assert
            Assert.IsTrue(raised, "PropertyChanged event was not raised");
        }
    }
}
