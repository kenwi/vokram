using System;
using System.Collections.Generic;
using IrcDotNet;

namespace vokram.Core.Interfaces
{
    public interface ISubscriptionRepository
    {
        void SubscribeToAllMessages(Action<IrcMessageEventArgs> callback);
        void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback);
        List<Action<IrcMessageEventArgs>> GetSubscriptions(IrcMessageEventArgs message);
        void UnsubscribeAll();
    }
}
