using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using IrcDotNet;
using IrcDotNet.Collections;
using vokram.Core.Interfaces;

namespace vokram.Core.Repositories
{
    public class SubscriptionsRepository : ISubscriptionRepository
    {
        private Dictionary<string, Action<IrcMessageEventArgs>> Subscriptions { get; } =
            new Dictionary<string, Action<IrcMessageEventArgs>>();

        public void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback)
        {
            if (!Subscriptions.ContainsKey(trigger))
                Subscriptions.Add(trigger, callback);
        }

        public void SubscribeToAllMessages(Action<IrcMessageEventArgs> callback)
        {
            Subscriptions.Add("^(.?$|[^!].*)", callback);
        }

        public List<Action<IrcMessageEventArgs>> GetSubscriptions(IrcMessageEventArgs message)
        {
            var list = new List<Action<IrcMessageEventArgs>>();
            Subscriptions.ForEach(subscription =>
            {
                if (Regex.IsMatch(message.Text, subscription.Key))
                    list.Add(subscription.Value);
            });
            return list;
        }
    }
}
