using System;
using System.IO;
using System.Linq;
using IrcDotNet.Collections;
using vokram.Plugins.MarkovBrainPlugin;

namespace vokram.Trainer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Train(args.FirstOrDefault() ?? "training.txt", args.Skip(1).FirstOrDefault() ?? "vokram.txt");
            Load("vokram.txt");

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        public static void Train(string trainingFile, string brainFile)
        {
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new MarkovChainTrainer(markovChainString);
            var saveBehaviour = new SaveBehaviour(markovChainString, brainFile);

            int i = 0;
            var messages = File.ReadAllLines(trainingFile);
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

        public static void Load(string brainFile)
        {
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new MarkovChainTrainer(markovChainString);
            var loadBehaviour = new LoadBehaviour(markovChainString, brainFile);
            var talkBehaviour = new TalkBehaviour(markovChainString);

            loadBehaviour.Process();
            var sample = talkBehaviour.GenerateRandomSentence();

            Console.WriteLine(sample);
        }
    }
}
