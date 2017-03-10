using System;
using System.Collections.Generic;
using IrcDotNet;

namespace vokram
{
    public interface IIrcBot
    {
        IList<IIrcPlugin> Plugins { get; }

        void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback);
        void SendMessage(IrcMessageEventArgs message);
    }
}