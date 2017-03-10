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
            _name = nick;
            RegistrationInfo = new IrcUserRegistrationInfo()
            {
                NickName = nick,
                UserName = "Markov",
                RealName = "Is Real"
            };

            Plugins = new List<IIrcPlugin>
            {
                new Help(),
                new MarkovBrain()
            };
            Plugins.ForEach(plugin => plugin.Initialize(this));

            MessageReceived += OnMessageReceived;
            PrivateMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, IrcMessageEventArgs message)
        {
            var list = new List<Action<IrcMessageEventArgs>>();
            _subscriptions.ForEach(subscription =>
            {
                if (Regex.IsMatch(message.Text, subscription.Key))
                    list.Add(subscription.Value);
            });
            list.ForEach( callback => callback(message));
        }

        public void SubscribeToMessage(string trigger, Action<IrcMessageEventArgs> callback)
        {
            if (!_subscriptions.ContainsKey(trigger))
                _subscriptions.Add(trigger, callback);
        }
    }
}