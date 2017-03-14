using System;
using System.Collections.Generic;
using IrcDotNet;
using IrcDotNet.Collections;

using vokram.Interfaces;
using vokram.Plugins;
using vokram.Repositories;

namespace vokram
{
    public class VokramBot : BasicClient, IIrcBot
    {
        private ISubscriptionRepository SubscriptionsRepository { get; } 
            = new SubscriptionsRepository();

        public IList<IIrcPlugin> Plugins { get; }
        public IrcUser Owner { get; set; }

        private string _name;
        public string Name {
            get { return _name; }
            set
            {
                _name = value;
                this.ChangeNick(_name);
            }
        }

        public VokramBot(string host, string nick) : base(host)
        {
            RegistrationInfo = SetupIdentity(nick);
            Plugins = SetupPlugins();
            Plugins.ForEach(plugin => plugin.Initialize(this));
            SetupEvents();
        }

        private IrcUserRegistrationInfo SetupIdentity(string nick)
        {
            _name = nick;
            return new IrcUserRegistrationInfo()
            {
                NickName = nick,
                UserName = "Markov",
                RealName = "Is Real"
            };
        }

        private List<IIrcPlugin> SetupPlugins()
        {
            return new List<IIrcPlugin>
            {
                new Help(),
                new MarkovBrain(),
                new Uptime()
            };
        }

        private void SetupEvents()
        {
            MessageReceived += OnMessageReceived;
            PrivateMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, IrcMessageEventArgs message)
        {
            var subscriptions = SubscriptionsRepository.GetSubscriptions(message);
            subscriptions.ForEach(callback => callback(message));
        }

        public void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback)
            => SubscriptionsRepository.SubscribeToMessage(trigger, callback);

        public void SubscribeToAllMessages(Action<IrcMessageEventArgs> callback)
            => SubscriptionsRepository.SubscribeToAllMessages(callback);
    }
}