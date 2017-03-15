﻿using System;
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
            //Bot.SubscribeToAllMessages(TrainCallback);
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
            var channel = message.Source.Name;
            var talker = new TalkBehaviour(_markovChainString);
            var sentence = talker.GenerateRandomSentence();

            var parameters = GetParameters(message.Text);
            ReinitializeFromParameters(talker, ref channel, ref sentence, parameters);

            var reply = message.CreateReply(sentence);
            SendMessage(channel, reply);
        }

        private static void ReinitializeFromParameters(TalkBehaviour talker, ref string channel, ref string sentence, string[] parameters)
        {
            if (parameters.Length >= 3 && parameters[1].ToLower() == "about")
            {
                var talkAboutText = parameters.Skip(2);

                if (talkAboutText.Last().StartsWith("#"))
                    channel = talkAboutText.Last();

                talkAboutText = RemoveChannelName(talkAboutText);
                sentence = talker.GenerateRandomSentenceFrom(talkAboutText);
            }
        }

        private void SendMessage(string channel, IrcMessageEventArgs reply)
        {
            if (reply.Text.Split(' ').Length > 2)
                Bot.SendTextToChannel(channel, reply.Text);
        }

        private static IEnumerable<string> RemoveChannelName(IEnumerable<string> talkAboutText)
        {
            talkAboutText = talkAboutText.Reverse().Skip(1).Reverse();
            return talkAboutText;
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