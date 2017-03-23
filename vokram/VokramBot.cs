using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrcDotNet;
using IrcDotNet.Collections;
using vokram.Core.Interfaces;
using vokram.Core.Repositories;
using vokram.Core.Client;

namespace vokram
{
    public class VokramBot : BasicClient, IIrcBot
    {
        public IrcUser Owner { get; set; }
        public string Name { get; set; }
        public IList<IIrcPlugin> Plugins { get; set; }

        public ISubscriptionRepository SubscriptionsRepository { get; }
            = new SubscriptionsRepository();

        public VokramBot(string host, string nick) : base(host)
        {
            RegistrationInfo = SetupIdentity(nick);
            Plugins = SetupPlugins();
            Plugins.ForEach(plugin => plugin.Initialize(this));
            SetupEvents();
        }

        private class ConsoleSource : IIrcMessageSource
        {
            public string Name { get; set; } = "Console";
        }

        private class ConsoleTarget : IIrcMessageTarget
        {
            public string Name { get; set; } = "Console";
        }

        private class ConsoleMessage : IrcMessageEventArgs
        {
            public ConsoleMessage(IIrcMessageSource source, IList<IIrcMessageTarget> targets,
                string text, Encoding encoding)
                : base(source, targets, text, encoding)
            {

            }

            public ConsoleMessage(string text) : base(new ConsoleSource(),
                new List<IIrcMessageTarget>(){new ConsoleTarget()},
                text,
                Encoding.Default)
            {

            }
        }

        protected override void MainLoop()
        {
            var input = Console.ReadLine();
            var message = new ConsoleMessage(input);
            MessageReceived?.Invoke(this, message);
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
            return Core.Utils.Plugins.LoadAll();
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
                //SendMessage(message.CreateReply(e.Message));
                Console.WriteLine(e.Message);
            }
        }

        public void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback)
            => SubscriptionsRepository.SubscribeToMessage(trigger, callback);

        public void SubscribeToAllMessages(Action<IrcMessageEventArgs> callback)
            => SubscriptionsRepository.SubscribeToAllMessages(callback);

        public void UnSubscribeAllMessages()
            => SubscriptionsRepository.UnsubscribeAll();

        public void SubscribeToJoinEvents(string channel)
        {
            var selectedChannel = this.DefaultClient.Channels.SingleOrDefault(c => c.Name.Equals(channel));
            selectedChannel.UserJoined += (s, e) => { };
        }
    }
}