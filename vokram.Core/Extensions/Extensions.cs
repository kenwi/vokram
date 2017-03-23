using IrcDotNet;

namespace vokram.Core.Extensions
{
    public static class Extensions
    {
        public static IrcMessageEventArgs CreateReply(this IrcMessageEventArgs message, string text)
        {
            return new IrcMessageEventArgs(message.Source, message.Targets, text, message.Encoding);
        }
    }
}