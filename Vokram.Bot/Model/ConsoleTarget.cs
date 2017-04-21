using IrcDotNet;

namespace Vokram.Bot.Model
{
    public class ConsoleTarget : IIrcMessageTarget
    {
        public string Name { get; set; } = "Consoles";

    }
}