using IrcDotNet;
using System.Text.RegularExpressions;

namespace vokram
{
    public static class Extensions
    {
        public static IrcMessageEventArgs CreateReply(this IrcMessageEventArgs message, string text)
        {
            return new IrcMessageEventArgs(message.Source, message.Targets, text, message.Encoding);
        }
    }
}