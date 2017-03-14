using System.Collections.Generic;
using System.Linq;
using IrcDotNet;

namespace vokram.Plugins
{
    public class MarkovBrain : PluginBase
    {
        private readonly MarkovChainString _markovChainString = new MarkovChainString();
        private readonly MarkovChainTrainer _markovChainTrainer;

        public MarkovBrain()
        {
            _markovChainTrainer = new MarkovChainTrainer(_markovChainString);
        }

        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!talk", TalkCallback);
            Bot.SubscribeToMessage("^!load", LoadCallback);
            Bot.SubscribeToMessage("^!save", SaveCallback);
            Bot.SubscribeToAllMessages(TrainCallback);
        }

        private void TrainCallback(IrcMessageEventArgs message)
        {
            _markovChainTrainer.Train(message.Text);
        }

        private void SaveCallback(IrcMessageEventArgs message)
        {
            var brainFile = GetBrainFile(message);
            var saver = new SaveBehaviour(_markovChainString, brainFile);
            saver.Process();

            var reply = message.CreateReply($"Saved brain '{brainFile}'");
            Bot.SendMessage(reply);
        }

        private void LoadCallback(IrcMessageEventArgs message)
        {
            var brainFile = GetBrainFile(message);
            var loader = new LoadBehaviour(_markovChainString, brainFile);
            loader.Process();

            var reply = message.CreateReply($"Loaded brain '{brainFile}'");
            Bot.SendMessage(reply);
        }

        private void TalkCallback(IrcMessageEventArgs message)
        {
            var talker = new TalkBehaviour(_markovChainString);

            // talk about
            var parameters = GetParameters(message.Text);
            if (parameters.Length >= 3 && parameters[1].ToLower() == "about")
            {
                message = message.CreateReply(talker.GenerateRandomSentenceFrom(parameters[2]));
                if (parameters.Length == 4) // channel
                {
                    var channel = parameters[3].StartsWith("#") ? parameters[3] : $"#{parameters[3]}";
                    Bot.SendTextToChannel(channel, message.Text);
                    return;
                }

                if(message.Text.Split(' ').Length > 2)
                    Bot.SendMessage(message);

                return;
            }

            message = message.CreateReply(talker.GenerateRandomSentence());
            if(message.Text.Split(' ').Length > 2)
                Bot.SendMessage(message);
        }

        private string GetBrainFile(IrcMessageEventArgs message)
        {
            var brainFile = Bot.Name;
            var command = message.Text.Split(' ');
            if (command.Length == 2)
            {
                brainFile = command[1];
            }
            return brainFile;
        }

        private string[] GetParameters(string text)
        {
            return text.Split(' ');
        }
    }
}