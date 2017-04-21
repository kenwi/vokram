using System;
using System.Collections.Generic;

namespace Vokram.Bot
{
    using Core.Interfaces;
    using Plugins;
    using Plugins.MarkovBrain;
    using Bot.Model;

    internal class Program
    {
        
        public static void Main(string[] args)
        {
            var plugins = new List<IIrcPlugin>() { new Join(), new Leave(), new Joke(), new MarkovBrain(), new Launch() };
            using (var vokram = new VokramBot("irc.freenode.net", "vokram", plugins))
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