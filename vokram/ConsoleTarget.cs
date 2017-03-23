using IrcDotNet;

namespace vokram
{
    public class ConsoleTarget : IIrcMessageTarget
    {
        public string Name { get; set; } = "Console";
    }
}