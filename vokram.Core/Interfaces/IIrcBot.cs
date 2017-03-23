using System;
using System.Collections.Generic;
using IrcDotNet;
using vokram.Core.Client;

namespace vokram.Core.Interfaces
{
    public interface IIrcBot
    {
        IList<IIrcPlugin> Plugins { get; set; }
        string Name { get; }
        IrcUser Owner { get; }

        void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback);
        void SubscribeToAllMessages(Action<IrcMessageEventArgs> callback);
        //void SubscribeToJoinEvents(string channel, Action<IrcChannelEventArgs> callback);

        BasicClient SendMessage(IrcMessageEventArgs message);
        BasicClient SendTextToChannel(string channel, string text);
        BasicClient Join(string channel);
        BasicClient Leave(string channel);
        void UnSubscribeAllMessages();
    }
}