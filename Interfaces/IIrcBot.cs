using System;
using System.Collections.Generic;
using IrcDotNet;

namespace vokram
{
    public interface IIrcBot
    {
        IList<IIrcPlugin> Plugins { get; }
        string Name { get; }
        IrcUser Owner { get; }

        void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback);
        BasicClient SendMessage(IrcMessageEventArgs message);
    }
}