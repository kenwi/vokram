using IrcDotNet;
using vokram.Core.Extensions;

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
            var reply = message.CreateReply($"{message.Source.Name}: NO HELP FOR YOU!");
            Bot.SendMessage(reply);
        }
    }
}