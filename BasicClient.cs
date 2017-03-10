using System;
using System.Linq;
using IrcDotNet;

namespace vokram
{
    public class BasicClient : Client
    {
        public IrcClient DefaultClient => Clients.SingleOrDefault();
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
                    if (!IsConnected)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public BasicClient Join(string channel)
        {
            DefaultClient.Channels.Join(channel);
            return this;
        }

        public BasicClient Leave(string channel)
        {
            DefaultClient.Channels.Leave(channel);
            return this;
        }

        public BasicClient SendMessageTo(string channel, string text)
        {
            DefaultClient.LocalUser.SendMessage(channel, text);
            return this;
        }

        public BasicClient SendMessage(Message message)
        {
            DefaultClient.LocalUser.SendMessage(message.Targets, message.Text);
            return this;
        }
    }
}