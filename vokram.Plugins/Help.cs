using IrcDotNet;
using Vokram.Core.Extensions;

namespace Vokram.Plugins
{
    public class Help : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!help", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            var reply = message.CreateReply($"{message.Source.Name}: NO HELP FOR YOU! Foo!!");
            Bot.SendMessage(reply);
        }
    }
}