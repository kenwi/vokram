using System.Net;
using HtmlAgilityPack;
using IrcDotNet;
using Vokram.Core.Extensions;

namespace Vokram.Plugins
{
    public class Launch : PluginBase
    {
        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!launch view", Callback);
        }

        private void Callback(IrcMessageEventArgs message)
        {
            var web = new HtmlWeb();
            var document = web.Load("https://spaceflightnow.com/launch-schedule/");
            var launchdate = document.DocumentNode.SelectSingleNode("//span[@class='launchdate']");
            var mission = document.DocumentNode.SelectSingleNode("//span[@class='mission']");

            var reply = message.CreateReply($"Next launch mission '{mission.InnerText}' is scheduled for {launchdate.InnerText}.");
            Bot.SendMessage(reply);
        }
    }
}