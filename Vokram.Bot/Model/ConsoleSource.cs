using IrcDotNet;

namespace Vokram.Bot.Model
{
    public class ConsoleSource : IIrcMessageSource
    {
        public string Name { get; set; } = "Console";
    }
}