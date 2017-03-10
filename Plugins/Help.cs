using System;
using IrcDotNet;

namespace vokram.Plugins
{
    public class Help : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!help", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            var reply = new IrcMessageEventArgs(message.Source, message.Targets, "NO HELP FOR YOU!", message.Encoding);
            Bot.SendMessage(reply);
        }
    }
}