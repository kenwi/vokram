using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections;
using System.Collections.Generic;

using IrcDotNet;
using IrcDotNet.Collections;

namespace Vokram.Plugins.MarkovBrain
{
    using Core.Extensions;
    using Core.Utils;

    public class MarkovBrain : PluginBase
    {
        private MarkovChainString _markovChainString = new MarkovChainString();
        private Trainer _markovChainTrainer;

        public MarkovBrain()
        {
            _markovChainTrainer = new Trainer(_markovChainString);
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
            var channel = message.Targets.First().Name;
            var talker = new TalkBehaviour(_markovChainString);
            var sentence = talker.GenerateRandomSentence();

            var parameters = GetParameters(message.Text);
            ReinitializeFromParameters(talker, ref channel, ref sentence, parameters);

            var reply = message.CreateReply(sentence);
            SendMessage(channel, reply);
        }

        private static void ReinitializeFromParameters(TalkBehaviour talker, ref string channel, ref string sentence, IEnumerable<string> parameters)
        {
            if (parameters.Count() < 3 || parameters.Skip(1).ToString().ToLower() != "about")
                return;

            var talkAboutWords = RemoveFirstKeywords(2, parameters);
            if (talkAboutWords.Last().StartsWith("#"))
            {
                channel = parameters.Last();
                talkAboutWords = RemoveChannelName(talkAboutWords);
            }

            sentence = talker.GenerateRandomSentenceFrom(talkAboutWords);
        }

        private static IEnumerable<string> RemoveFirstKeywords(int count, IEnumerable<string> parameters)
        {
            return parameters.Skip(count);
        }

        private void SendMessage(string channel, IrcMessageEventArgs reply)
        {
            if (reply.Text.Split(' ').Length > 2)
                Bot.SendTextToChannel(channel, reply.Text);
        }

        private static string[] RemoveChannelName(IEnumerable<string> words)
        {
            return words.Reverse().Skip(1).Reverse().ToArray();
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

        private IEnumerable<string> GetParameters(string text)
        {
            return text.Split(' ');
        }

 
    }
}