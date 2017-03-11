using IrcDotNet;
using System;
using System.Collections.Generic;

namespace vokram.Interfaces
{
    public interface ISubscriptionRepository
    {
        void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback);
        List<Action<IrcMessageEventArgs>> GetSubscriptions(IrcMessageEventArgs message);
    }
}
