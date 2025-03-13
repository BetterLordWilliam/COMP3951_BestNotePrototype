using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunityToolkit.Mvvm.Messaging;
using BestNote_3951.Messages;
using BestNote_3951.ViewModels;

namespace COMP3951_BestNoteTests
{
    [TestClass]
    public class TestMarkdownEditor
    {
        private MarkdownEditorViewModel mdvm = new MarkdownEditorViewModel();

        [TestInitialize]
        public void Initialize()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);

            mdvm = new MarkdownEditorViewModel();
        }

        [TestMethod]
        public void TestEditorText()
        {
            // Arrange
            string? receivedMarkdown = null;

            // Register for the MarkdownTextChangedMessage on the default messenger
            WeakReferenceMessenger.Default.Register<TestMarkdownEditor, MarkdownTextChangedMessage>(
                this,
                (recipient, message) =>
                {
                    receivedMarkdown = message.Value;
                }
            );

            const string newText = "# Unit Test Markdown";

            // Act
            // Setting this property should invoke the partial OnMarkdownTextChanged 
            // method, which sends out a MarkdownTextChangedMessage.
            mdvm.MarkdownText = newText;

            // Assert
            Assert.AreEqual(newText, receivedMarkdown, "The updated Markdown text should have been sent via the message.");
        }
    }
}
