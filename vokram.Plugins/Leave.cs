using IrcDotNet;
using vokram.Core;

namespace vokram.Plugins
{
    public class Leave : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!leave", Callback);
        }

        private void Callback(IrcMessageEventArgs ircMessageEventArgs)
        {
            var channel = ircMessageEventArgs.Text.Split(' ')[1];
            channel = channel.StartsWith("#") ? channel : $"#{channel}";
            Bot.Leave(channel);
        }
    }
}