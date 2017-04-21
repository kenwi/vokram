using System.Collections.Generic;
using System.Text;
using IrcDotNet;

namespace Vokram.Bot.Model
{
    public class ConsoleMessage : IrcMessageEventArgs
    {
        public ConsoleMessage(IIrcMessageSource source, IList<IIrcMessageTarget> targets,
            string text, Encoding encoding)
            : base(source, targets, text, encoding)
        {

        }

        public ConsoleMessage(string text) : base(new ConsoleSource(),
            new List<IIrcMessageTarget>() { new ConsoleTarget() },
            text,
            Encoding.Default)
        {

        }
    }
}