using IrcDotNet;

namespace Vokram
{
    public class ConsoleTarget : IIrcMessageTarget
    {
        public string Name { get; set; } = "Console";
    }
}