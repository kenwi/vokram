using System;
using System.Linq;

namespace Vokram.Plugins.MarkovBrain.Trainer
{
    using Core.Utils;
    using Plugins.MarkovBrain;
        
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
                    Load = getArgumentValue(args, "--load") ?? "Logs/130494-herbert.freenode.net-/#nff.txt",
                    Save = getArgumentValue(args, "--save") ?? "vokram.txt",
                    Filter = getArgumentValue(args, "--filter") ?? "",
                    Sections = int.Parse(getArgumentValue(args, "--sections") ?? "1"),
                    Reports = int.Parse(getArgumentValue(args, "--reports") ?? "50"),
                    Samples = int.Parse(getArgumentValue(args, "--samples") ?? "25")
                };
                consoleLog(config.ToString());

                var brain = Trainer.Train(config, consoleLog);
                Trainer.Save(config, brain, consoleLog);
                Trainer.Load(config, consoleLog);
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
