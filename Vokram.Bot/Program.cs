using System;

namespace Vokram
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var vokram = new VokramBot("irc.freenode.net", "vokram"))
            {
                vokram.ClientRegistered = (sender, eventArgs) => vokram.Join("#hadamard");
                vokram.Tick = (sender, dt) =>
                {
                    var input = Console.ReadLine();
                    var message = new ConsoleMessage(input);
                    vokram.MessageReceived(sender, message);
                };
                vokram.ConnectAndEnterMainLoop();
            }
        }
    }
}