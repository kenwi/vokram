using IrcDotNet;

namespace vokram
{
    public class ConsoleSource : IIrcMessageSource
    {
        public string Name { get; set; } = "Console";
    }
}