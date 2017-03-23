using System;
using System.Collections.ObjectModel;
using System.Threading;
using IrcDotNet;
using IrcDotNet.Collections;

namespace vokram.Core.Client
{
    public abstract class Client : IDisposable
    {
        private bool _isDisposed;
        protected readonly Collection<IrcDotNet.IrcClient> Clients = new Collection<IrcDotNet.IrcClient>();

        public EventHandler<EventArgs> ClientConnected;
        public EventHandler<EventArgs> ClientRegistered;
        public EventHandler<EventArgs> ClientDisconnected;
        public EventHandler<IrcMessageEventArgs> MessageReceived;
        public EventHandler<IrcMessageEventArgs> PrivateMessageReceived;
        public EventHandler<IrcChannelEventArgs> JoinedChannel;
        public EventHandler<IrcChannelEventArgs> LeftChannel;

        public IrcRegistrationInfo RegistrationInfo { get; set; }

        public void Connect(string server)
        {
            if(string.IsNullOrEmpty(server))
                throw new NullReferenceException("Server parameter must not be null.");

            if (RegistrationInfo == null)
                throw new NullReferenceException("RegistrationInfo must not be null on Connect(string);");

            var client = SetupEventsAndCreateClient();
            if (!ConnectToServer(client, server))
                throw new Exception($"Could not connect to '{server}'");

            Clients.Add(client);
            Console.Out.WriteLine("Now connected to '{0}'.", server);
        }

        private bool ConnectToServer(StandardIrcClient client, string server)
        {
            // Wait until connection has succeeded or timed out.
            using (var connectedEvent = new ManualResetEventSlim(false))
            {
                client.Connected += (sender2, e2) => connectedEvent.Set();
                client.Connect(server, false, RegistrationInfo);
                if (!connectedEvent.Wait(10000))
                {
                    client.Dispose();
                    //ConsoleUtilities.WriteError("Connection to '{0}' timed out.", server);
                    return false;
                }
            }
            return true;
        }

        private StandardIrcClient SetupEventsAndCreateClient()
        {
            var client = new StandardIrcClient {FloodPreventer = new IrcStandardFloodPreventer(4, 2000)};
            client.Connected += (o, e) => ClientConnected?.Invoke(o, e);
            client.Disconnected += (o, e) => ClientDisconnected?.Invoke(o, e);
            client.Registered += (o, e) =>
            {
                var senderClient = (IrcDotNet.IrcClient) o;
                senderClient.LocalUser.MessageReceived += (sender, args) => PrivateMessageReceived?.Invoke(sender, args);
                senderClient.LocalUser.JoinedChannel += LocalUserOnJoinedChannel;
                senderClient.LocalUser.LeftChannel += LocalUserOnLeftChannel;
                ClientRegistered?.Invoke(o, e);
            };
            return client;
        }

        private void LocalUserOnJoinedChannel(object sender, IrcChannelEventArgs ircChannelEventArgs)
        {
            ircChannelEventArgs.Channel.MessageReceived += MessageReceived;
            JoinedChannel?.Invoke(sender, ircChannelEventArgs);
        }

        private void LocalUserOnLeftChannel(object sender, IrcChannelEventArgs ircChannelEventArgs)
        {
            ircChannelEventArgs.Channel.MessageReceived -= MessageReceived;
            LeftChannel?.Invoke(sender, ircChannelEventArgs);
        }

        public void Disconnect()
        {
            Clients.ForEach(c =>
            {
                c.Quit(1000, "");
                c.Dispose();
            });
            Clients.Clear();
        }

        ~Client()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_isDisposed)
                if (disposing)
                    foreach (var client in Clients)
                        if (client != null)
                        {
                            client.Quit(1000, "");
                            client.Dispose();
                        }
            _isDisposed = true;
        }
    }
}
