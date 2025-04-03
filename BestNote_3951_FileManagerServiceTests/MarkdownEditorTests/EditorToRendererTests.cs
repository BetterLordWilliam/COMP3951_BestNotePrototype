using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestNote_3951.ViewModels;
using BestNote_3951.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace BestNote_3951_Tests.MarkdownEditorTests
{
    [TestClass]
    public class MarkdownEditorViewModelMessengerTests
    {
        [TestInitialize]
        public void Setup()
        {
            WeakReferenceMessenger.Default.Reset();
        }

        [TestMethod]
        public void ChangingMarkdownText_SendsMarkdownTextChangedMessage()
        {
            // Arrange
            var vm = new MarkdownEditorViewModel();
            string? receivedMarkdown = null;

            WeakReferenceMessenger.Default.Register<MarkdownTextChangedMessage>(this,
                (recipient, message) =>
                {
                    receivedMarkdown = message.Value;
                });

            // Act
            vm.MarkdownText = "## A new heading for test";

            // Assert
            Assert.AreEqual("## A new heading for test", receivedMarkdown,
                "Expected to receive the updated markdown text via MarkdownTextChangedMessage, but did not.");
        }
    }
}
