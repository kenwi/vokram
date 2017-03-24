using System;
using System.Linq;
using vokram.Plugins;

namespace vokram.Trainer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var traininfFile = GetArgumentValue(args, "-t") ?? "training.txt";
            var brainFile = GetArgumentValue(args, "-b") ?? "vokram.txt";

            MarkovBrain.Train(traininfFile, brainFile, Console.WriteLine);
            MarkovBrain.Save(brainFile, Console.WriteLine);
            MarkovBrain.Load(brainFile, Console.WriteLine);

            Console.WriteLine("Done.");
            Console.Beep();
            Console.ReadKey();
        }

        private static string GetArgumentValue(string[] args, string parameter)
        {
            return args.SingleOrDefault(argument => argument.Contains(parameter))?.Split('=').Last();
        }
    }
}
