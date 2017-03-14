using IrcDotNet;

namespace vokram.Plugins
{
    public class Join : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!join", Callback);
        }

        private void Callback(IrcMessageEventArgs ircMessageEventArgs)
        {
            var channel = ircMessageEventArgs.Text.Split(' ')[1];
            channel = channel.StartsWith("#") ? channel : $"#{channel}";
            Bot.Join(channel);
        }
    }
}