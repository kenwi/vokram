# Vokram.Plugins
The functionality of the bot is extended by plugins. Plugins will in the future be loaded dynamically as dll's at runtime, which will enable loading and reloading of hot updated plugins.

## Vokram.Bot
Plugins can be issued as a collection to the constructor of the bot. If no plugin argument is given, it will for now just load the `Join` and `Leave` plugins.
```C#
public static void Main(string[] args)
{
    var plugins = new List<IIrcPlugin>() { new Join(), new Leave(), new Joke(), new MarkovBrain(), new Launch() };
    using (var vokram = new VokramBot("irc.freenode.net", "vokram", plugins))
    {
        vokram.ConnectAndEnterMainLoop();
    }
}
```

## Joke.cs
Plugins inherit from the `PluginBase` class and implements the `Initialize` method.
```C#
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
```
