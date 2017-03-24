using System;
using System.Linq;
using vokram.Plugins;

namespace vokram.Trainer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var traininfFile = args.FirstOrDefault() ?? "training.txt";
            var brainFile = args.Skip(1).FirstOrDefault() ?? "vokram.txt";

            MarkovBrain.Train(traininfFile, brainFile, Console.WriteLine);
            MarkovBrain.Save(brainFile, Console.WriteLine);
            MarkovBrain.Load(brainFile, Console.WriteLine);

            Console.WriteLine("Done.");
            Console.Beep();
            Console.ReadKey();
        }
    }
}
