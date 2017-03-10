﻿using IrcDotNet;

namespace vokram
{
    public static class Extensions
    {
        public static IrcMessageEventArgs CreateReply(this IrcMessageEventArgs message, string text)
        {
            return new IrcMessageEventArgs(message.Source, message.Targets, message.Text, message.Encoding);
        }
    }
}