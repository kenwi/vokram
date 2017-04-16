using System;
using System.Linq;
using Vokram.Core.Utils;
using Vokram.Plugins;
using Vokram.Plugins.MarkovBrainPlugin;

namespace Vokram.Trainer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var consoleLog = new Action<string>
                (logevent => Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {logevent}"));

            var getArgumentValue = new Func<string[], string, string>
                ((arguments, parameter) => arguments.SingleOrDefault(argument => argument.ToLower().Contains(parameter))?.Split('=').Last());

            try
            {
                var config = new Config {
                    TrainingFile = getArgumentValue(args, "--trainingfile") ?? "Logs/sample.txt",
                    BrainFile = getArgumentValue(args, "--brainfile") ?? "vokram.txt",
                    LogSections = int.Parse(getArgumentValue(args, "--logsections") ?? "1"),
                    NumReports = int.Parse(getArgumentValue(args, "--reports") ?? "100"),
                    NumSamples = int.Parse(getArgumentValue(args, "--samples") ?? "10")
                };
                consoleLog(config.ToString());

                //var brain = MarkovBrain.Train(config, consoleLog);
                var brain = MarkovChainTrainer.Train(config, consoleLog);
                MarkovBrain.Save(config.BrainFile, brain, consoleLog);
                MarkovBrain.Load(config, consoleLog);
                consoleLog("Done");
            }
            catch (Exception ex)
            {
                consoleLog(ex.Message);
                Console.Beep();
            }
            Console.Beep();
            ;
        }
    }
}
