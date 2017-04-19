using System;
using System.Collections.Generic;
using System.Linq;
using IrcDotNet;
using IrcDotNet.Collections;

namespace Vokram
{
    using Core.Interfaces;
    using Core.Repositories;
    using Core.Client;
    using Plugins;

    public class VokramBot : BasicClient, IIrcBot
    {
        public IrcUser Owner { get; set; }
        public string Name { get; set; }
        public IList<IIrcPlugin> Plugins { get; set; }
        public ISubscriptionRepository SubscriptionsRepository { get; } = new SubscriptionsRepository();
        
        public VokramBot(string host, string nick, IList<IIrcPlugin> plugins = null) : base(host)
        {
            RegistrationInfo = SetupIdentity(nick);
            Plugins = plugins ?? SetupPlugins();
            Plugins.ForEach(plugin => plugin.Initialize(this));
            SetupEvents();
        }

        private IrcUserRegistrationInfo SetupIdentity(string nick)
        {
            Name = nick;
            return new IrcUserRegistrationInfo()
            {
                NickName = nick,
                UserName = "Markov",
                RealName = "Is Real"
            };
        }

        private static List<IIrcPlugin> SetupPlugins()
        {
            return new List<IIrcPlugin>()
            {
                new Join(),
                new Leave()
            };
            //return Core.Utils.Plugins.LoadAll();
        }

        private void SetupEvents()
        {
            MessageReceived += OnMessageReceived;
            PrivateMessageReceived += OnMessageReceived;
        }

        protected void OnMessageReceived(object sender, IrcMessageEventArgs message)
        {
            try
            {
                var subscriptions = SubscriptionsRepository.GetSubscriptions(message);
                subscriptions.ForEach(callback => callback(message));
            }
            catch (Exception e)
            {
                var messageConsoleRedirect = new ConsoleMessage(e.Message);
                SendMessage(messageConsoleRedirect);
            }
        }

        public void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback)    => SubscriptionsRepository.SubscribeToMessage(trigger, callback);
        public void SubscribeToAllMessages(Action<IrcMessageEventArgs> callback)                => SubscriptionsRepository.SubscribeToMessage("^(.?$|[^!].*)", callback);
        public void UnSubscribeAllMessages()                                                    => SubscriptionsRepository.UnsubscribeAll();

        public void SubscribeToJoinEvents(string channel)
        {
            var selectedChannel = this.DefaultClient.Channels.SingleOrDefault(c => c.Name.Equals(channel));
            selectedChannel.UserJoined += (s, e) => { };
        }
    }
}
