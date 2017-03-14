using System;
using System.Collections.Generic;
using IrcDotNet;
using vokram.Interfaces;

namespace vokram
{
    public interface IIrcBot
    {
        IList<IIrcPlugin> Plugins { get; }
        string Name { get; }
        IrcUser Owner { get; }

        void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback);
        void SubscribeToAllMessages(Action<IrcMessageEventArgs> callback);

        BasicClient SendMessage(IrcMessageEventArgs message);
        BasicClient SendTextToChannel(string channel, string text);
        BasicClient Join(string channel);
        BasicClient Leave(string channel);
    }
}