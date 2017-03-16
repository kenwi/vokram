﻿using System;
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
        public IrcUser Owner { get; set; }
        public string Name { get; set; }
        public IList<IIrcPlugin> Plugins { get; }
        public ISubscriptionRepository SubscriptionsRepository { get; }
            = new SubscriptionsRepository();

        public VokramBot(string host, string nick) : base(host)
        {
            RegistrationInfo = SetupIdentity(nick);
            Plugins = SetupPlugins();
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
            return new List<IIrcPlugin>
            {
                new Help(), new MarkovBrain(), new Uptime(),
                new Join(), new Leave(), new Say(), new Joke()
            };
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
    }
}