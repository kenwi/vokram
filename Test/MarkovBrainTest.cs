using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using IrcDotNet;
using IrcDotNet.Collections;
using NUnit.Framework;
using vokram;
using vokram.Plugins;

namespace Test
{
    [TestFixture]
    public class MarkovBrainTest
    {
         public class Source : IIrcMessageSource
        {
            public string Name { get; } = "m0b";
        }

        public class Target : IIrcMessageTarget
        {
            public string Name { get; } = "#hadamard";
        }

        [Test]
        public void LoadBrainAndRunTalkCommand()
        {
            var bot = new VokramBot("127.0.0.1", "vokram");
            bot.OfflineOnly = true;

            var brain = new MarkovBrain();
            brain.Initialize(bot);

            Environment.CurrentDirectory = "/home/kenwi/Desktop/Hadamard/Test";
            var targets = new List<IIrcMessageTarget>() {new Target()};
            var source = new Source();
            var message = new IrcMessageEventArgs(source, targets, "!load", Encoding.ASCII);
            bot.MessageReceived(null, message);

            Enumerable.Range(0, 50)
                .ForEach(i =>
                {
                    message = new IrcMessageEventArgs(source, targets, "!talk", Encoding.ASCII);
                    bot.MessageReceived(null, message);
                });
        }
        [Test]
        public void LoadBrainAndRunTalkAboutCommand()
        {
            var bot = new VokramBot("127.0.0.1", "vokram");
            bot.OfflineOnly = true;

            var brain = new MarkovBrain();
            brain.Initialize(bot);

            Environment.CurrentDirectory = "/home/kenwi/Desktop/Hadamard/Test";
            var targets = new List<IIrcMessageTarget>() {new Target()};
            var source = new Source();
            var message = new IrcMessageEventArgs(source, targets, "!load", Encoding.ASCII);
            bot.MessageReceived(null, message);

            Enumerable.Range(0, 10)
                .ForEach(i =>
                {

                    message = new IrcMessageEventArgs(source, targets, "!talk", Encoding.ASCII);
                    bot.MessageReceived(null, message);
                });

            Enumerable.Range(0, 10)
                .ForEach(i =>
                {

                    message = new IrcMessageEventArgs(source, targets, "!talk about alle har jo, Encoding.ASCII);
                    bot.MessageReceived(null, message);
                });

            Enumerable.Range(0, 10)
                .ForEach(i =>
                {
                    message = new IrcMessageEventArgs(source, targets, "!talk about en slik #nff", Encoding.ASCII);
                    bot.MessageReceived(null, message);
                });
        }
    }
}
