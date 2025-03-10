using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BestNote_3951.Messages
{
    public class MarkdownTextChangedMessage : ValueChangedMessage<string>
    {
        public MarkdownTextChangedMessage(string value) : base(value)
        {

        }
    }
}
