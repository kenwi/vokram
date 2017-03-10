﻿using System;
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
            Bot.SubscribeToMessage("^(?!/!)\\W+$", TrainCallback);
        }

        private void TrainCallback(IrcMessageEventArgs message)
        {

        }

        private void SaveCallback(IrcMessageEventArgs message)
        {
            var saver = new SaveBehaviour(_markovChainString) {BrainFile = "vokram"};
            saver.Process();
        }

        private void LoadCallback(IrcMessageEventArgs message)
        {
            var loader = new LoadBehaviour(_markovChainString) {BrainFile = "vokram"};
            loader.Process();
        }

        private void TalkCallback(IrcMessageEventArgs message)
        {
            var talker = new TalkBehaviour(_markovChainString);
            var reply = message.CreateReply(talker.GenerateRandomSentence());
        }
    }
}