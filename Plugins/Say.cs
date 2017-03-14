using IrcDotNet;

namespace vokram.Plugins
{
    public class Say : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!say", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            var text = message.Text;
            var channel = text.Split(' ')[1];
            text = text.Replace($"!say {channel} ", "");
            channel = channel.StartsWith("#") ? channel : $"#{channel}";
            Bot.SendTextToChannel(channel, text);
        }
    }
}