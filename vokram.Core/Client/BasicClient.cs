﻿using System;
using System.Linq;
using IrcDotNet;

namespace vokram.Core.Client
{
    public abstract class BasicClient : Client
    {
        public bool OfflineOnly { get; set; } = false;

        public IrcDotNet.IrcClient DefaultClient => Clients.SingleOrDefault();
        public bool IsConnected => Clients.Any();

        private readonly string _host;

        protected BasicClient(string host)
        {
            _host = host;
        }

        public void ConnectAndEnterMainLoop()
        {
            try
            {
                Connect(_host);
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    MainLoop();
                    if (!IsConnected)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected abstract void MainLoop();

        public BasicClient Join(string channel)
        {
            if (OfflineOnly)
            {
                Console.WriteLine($"Joined #{channel}");
                return this;
            }
            DefaultClient.Channels.Join(channel);
            return this;
        }

        public BasicClient Leave(string channel)
        {
            if (OfflineOnly)
            {
                Console.WriteLine($"Left #{channel}");
                return this;
            }
            DefaultClient.Channels.Leave(channel);
            return this;
        }

        public BasicClient ChangeNick(string name)
        {
            if (OfflineOnly)
            {
                Console.WriteLine($"Changed nick '{name}'");
                return this;
            }
            DefaultClient.LocalUser.SetNickName(name);
            return this;
        }

        public BasicClient SendMessage(IrcMessageEventArgs message)
        {
            if (OfflineOnly || message.Source.Name.Equals("Console"))
            {
                Console.WriteLine($"{message.Text}");
                return this;
            }

            DefaultClient.LocalUser.SendMessage(message.Targets, message.Text);
            return this;
        }

        public BasicClient SendTextToChannel(string channel, string text)
        {
            if (OfflineOnly)
            {
                Console.WriteLine($"{channel}: {text}");
                return this;
            }
            DefaultClient.LocalUser.SendMessage(channel, text);
            return this;
        }
    }
}