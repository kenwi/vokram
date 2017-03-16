using System;
using System.IO;
using IrcDotNet;
using IrcDotNet.Collections;
using vokram.Plugins;

namespace vokram
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var vokram = new VokramBot("irc.freenode.net", "vokram"))
            {
                vokram.ClientRegistered = (sender, eventArgs) => vokram.Join("#hadamard");
                vokram.ConnectAndEnterMainLoop();
            }
        }

        public static void Train()
        {
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new MarkovChainTrainer(markovChainString);
            var saveBehaviour = new SaveBehaviour(markovChainString, "vokram");

            int i = 0;
            var messages = File.ReadAllLines("training.txt");
            messages.ForEach(message =>
            {
                var datetime = message.Substring(0, 21);
                if (i++ % 1000 == 0)
                    Console.WriteLine(datetime);

                message = message.Remove(0, 22);
                if (message.StartsWith("<"))
                {
                    message = message.Split('>')[1];
                    message = message.Remove(0, 1);

                    markovChainTrainer.Train(message);
                }
            });

            saveBehaviour.Process();
        }

        public static void Load()
        {
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new MarkovChainTrainer(markovChainString);
            var loadBehaviour = new LoadBehaviour(markovChainString, "vokram");
            var talkBehaviour = new TalkBehaviour(markovChainString);

            loadBehaviour.Process();
            var sample = talkBehaviour.GenerateRandomSentence();

            Console.WriteLine(sample);
        }
    }
}
