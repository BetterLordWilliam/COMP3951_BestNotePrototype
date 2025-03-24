using BestNote_3951.Messages;
using BestNote_3951.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BestNote_3951.Views;

public partial class MarkdownEditorView : ContentView
{
    private string _previous;
    private DateTime _coolDown;

	public MarkdownEditorView()
	{
		InitializeComponent();

		WeakReferenceMessenger.Default.Register<InsertAtCursorMessage>(this, (recipient, message) =>
		{
            if (message.Value == _previous && (DateTime.UtcNow - _coolDown).TotalSeconds < 1)
            {
                return;
            }
            _previous = message.Value;
            _coolDown = DateTime.UtcNow;

            string original = EditorControl.Text ?? string.Empty;
            int cursorPosition = EditorControl.CursorPosition;

            if (cursorPosition < 0 || cursorPosition > original.Length)
            {
                cursorPosition = original.Length;
            }

            string newText = original.Substring(0, cursorPosition) +
                message.Value + original.Substring(cursorPosition);

            EditorControl.Text = newText;

            EditorControl.CursorPosition = cursorPosition + message.Value.Length;

        });
	}
}