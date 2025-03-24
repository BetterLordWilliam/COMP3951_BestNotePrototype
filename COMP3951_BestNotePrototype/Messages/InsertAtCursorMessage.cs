using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BestNote_3951.Messages
{
    public class InsertAtCursorMessage : ValueChangedMessage<string>
    {
        public InsertAtCursorMessage(string value) : base(value)
        {

        }
    }
}
