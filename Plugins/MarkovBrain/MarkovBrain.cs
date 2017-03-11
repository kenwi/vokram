using System;
using IrcDotNet;

namespace vokram.Plugins
{
    public class MarkovBrain : PluginBase
    {
        private readonly MarkovChainString _markovChainString = new MarkovChainString();

        protected override void Initialize()
        {
            Bot.SubscribeToMessage("^!talk", TalkCallback);
            Bot.SubscribeToMessage("^!load", LoadCallback);
            Bot.SubscribeToMessage("^!save", SaveCallback);
            Bot.SubscribeToAllMessages(TrainCallback);
        }

        private void TrainCallback(IrcMessageEventArgs message)
        {
            _markovChainString.Train(message);
        }

        private void SaveCallback(IrcMessageEventArgs message)
        {
            var saver = new SaveBehaviour(_markovChainString, Bot.Name);
            saver.Process();
        }

        private void LoadCallback(IrcMessageEventArgs message)
        {
            var loader = new LoadBehaviour(_markovChainString, Bot.Name);
            loader.Process();
        }

        private void TalkCallback(IrcMessageEventArgs message)
        {
            var talker = new TalkBehaviour(_markovChainString);
            var reply = message.CreateReply(talker.GenerateRandomSentence());
            Bot.SendMessage(reply);
        }
    }
}