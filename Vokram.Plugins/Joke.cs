using System.Net;
using HtmlAgilityPack;
using IrcDotNet;
using Vokram.Core;
using Vokram.Core.Extensions;
using System;

namespace Vokram.Plugins
{
    public class Joke : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!joke", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            Func<string, string> stripJunk = (text) => text.Replace("\n", "").Replace("\r", "").Trim(' ');
            var web = new HtmlWeb();
            var document = web.Load("http://schneierfacts.com");
            var node = document.DocumentNode.SelectSingleNode("//div[@class='fact']");
            var joke = WebUtility.HtmlDecode(stripJunk(node.InnerText));

            var reply = message.CreateReply(joke);
            Bot.SendMessage(reply);
        }
    }
}