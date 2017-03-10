using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IrcDotNet;
using IrcDotNet.Collections;
using vokram.Plugins;

namespace vokram
{
    public class VokramBot : BasicClient, IIrcBot
    {
        private readonly Dictionary<string, Action<IrcMessageEventArgs>> _subscriptions
            = new Dictionary<string, Action<IrcMessageEventArgs>>();
        
        public IList<IIrcPlugin> Plugins { get; }

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
            RegistrationInfo = new IrcUserRegistrationInfo()
            {
                NickName = Name,
                UserName = "Markov",
                RealName = "Is Real"
            };

            Plugins = new List<IIrcPlugin>
            {
                new Help(),
                new MarkovBrain()
            };

            MessageReceived += OnMessageReceived;
            PrivateMessageReceived += OnMessageReceived;

            Plugins.ForEach(plugin => plugin.Initialize(this));
        }

        private void OnMessageReceived(object sender, IrcMessageEventArgs message)
        {
            var list = new List<Action<IrcMessageEventArgs>>();
            foreach (var subscription in _subscriptions)
            {
                if (Regex.IsMatch(message.Text, subscription.Key))
                    list.Add(subscription.Value);
            }
            list.ForEach( callback => callback(message));
        }

        public void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback)
        {
            if (!_subscriptions.ContainsKey(trigger))
                _subscriptions.Add(trigger, callback);
        }
    }
}