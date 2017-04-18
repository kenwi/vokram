using System;
using System.Collections.Generic;
using Vokram.Core.Interfaces;
using Vokram.Plugins;

namespace Vokram
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var plugins = new List<IIrcPlugin>() { new Joke(), new MarkovBrain() };
            using (var vokram = new VokramBot("irc.freenode.net", "vokram2"))
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