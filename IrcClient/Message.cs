using System.Collections.Generic;
using IrcDotNet;

namespace vokram
{
    public class Message
    {
        public IList<IIrcMessageTarget> Targets;
        public string Text;

        public static Message CreateReplyMessage(IList<IIrcMessageTarget> targets, string text)
        {
            return new Message()
            {
                Targets = targets,
                Text = text
            };
        }
    }
}