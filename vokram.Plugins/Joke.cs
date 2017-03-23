using System.Net;
using HtmlAgilityPack;
using IrcDotNet;
using vokram.Core;
using vokram.Core.Extensions;

namespace vokram.Plugins
{
    public class Joke : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!joke", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            var web = new HtmlWeb();
            var document = web.Load("http://schneierfacts.com");
            var node = document.DocumentNode.SelectSingleNode("//div[@class='fact']");
            var joke = WebUtility.HtmlDecode(node.InnerText.Replace("\n", "").Replace("\r", "").Trim(' '));

            var reply = message.CreateReply(joke);
            Bot.SendMessage(reply);
        }
    }
}