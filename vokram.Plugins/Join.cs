using IrcDotNet;
using Vokram.Core;

namespace Vokram.Plugins
{
    public class Join : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!join", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            var channel = message.Text.Split(' ')[1];
            channel = channel.StartsWith("#") ? channel : $"#{channel}";
            Bot.Join(channel);
        }
    }
}