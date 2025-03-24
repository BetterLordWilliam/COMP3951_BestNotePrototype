using BestNote_3951.Messages;
using BestNote_3951.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BestNote_3951.Views;

public partial class MarkdownEditorView : ContentView
{
	public MarkdownEditorView()
	{
		InitializeComponent();

		WeakReferenceMessenger.Default.Register<InsertAtCursorMessage>(this, (recipient, message) =>
		{
			InsertTextAtCursor(message.Value);
		});
	}

	private void InsertTextAtCursor(string link)
	{
        string original = EditorControl.Text ?? string.Empty;
        int cursorPosition = EditorControl.CursorPosition;

        if (cursorPosition < 0 || cursorPosition > original.Length)
        {
            cursorPosition = original.Length;
        }

        string newText = original.Substring(0, cursorPosition) +
            link + original.Substring(cursorPosition);

		if (newText == original)
		{
			return;
		}

        EditorControl.Text = newText;
	}
}