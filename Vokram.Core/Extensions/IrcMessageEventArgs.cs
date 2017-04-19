using IrcDotNet;

namespace Vokram.Core.Extensions
{
    public static class Extensions
    {
        public static IrcMessageEventArgs CreateReply(this IrcMessageEventArgs message, string text)
        {
            return new IrcMessageEventArgs(message.Source, message.Targets, text, message.Encoding);
        }
    }
}
