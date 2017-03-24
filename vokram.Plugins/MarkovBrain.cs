﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using IrcDotNet;
using IrcDotNet.Collections;
using vokram.Core.Extensions;
using vokram.Plugins.MarkovBrainPlugin;

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
            var channel = message.Targets.First().Name;
            var talker = new TalkBehaviour(_markovChainString);
            var sentence = talker.GenerateRandomSentence();

            var parameters = GetParameters(message.Text);
            ReinitializeFromParameters(talker, ref channel, ref sentence, parameters);

            var reply = message.CreateReply(sentence);
            SendMessage(channel, reply);
        }

        private static void ReinitializeFromParameters(TalkBehaviour talker, ref string channel, ref string sentence, string[] parameters)
        {
            if (parameters.Length < 3 || parameters[1].ToLower() != "about")
                return;

            var talkAboutWords = RemoveFirstKeywords(2, parameters);
            if (talkAboutWords.Last().StartsWith("#"))
            {
                channel = parameters.Last();
                talkAboutWords = RemoveChannelName(talkAboutWords);
            }

            sentence = talker.GenerateRandomSentenceFrom(talkAboutWords);
        }

        private static string[] RemoveFirstKeywords(int count, string[] parameters)
        {
            return parameters.Skip(count).ToArray();
        }

        private void SendMessage(string channel, IrcMessageEventArgs reply)
        {
            if (reply.Text.Split(' ').Length > 2)
                Bot.SendTextToChannel(channel, reply.Text);
        }

        private static string[] RemoveChannelName(string[] words)
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

        private string[] GetParameters(string text)
        {
            return text.Split(' ');
        }

        public static MarkovChainString Train(string trainingFile, string brainFile, Action<string> outputAction)
        {
            outputAction?.Invoke($"Started training from '{trainingFile}'.");
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new MarkovChainTrainer(markovChainString);

            int i = 0;
            var messages = File.ReadAllLines(trainingFile);
            messages.ForEach(message =>
            {
                if (message.StartsWith("#"))
                    return;

                var datetime = message.Substring(0, 21);
                if (i++ % 1000 == 0)
                    outputAction?.Invoke(datetime);

                message = message.Remove(0, 22);
                if (message.StartsWith("<"))
                {
                    message = message.Split('>')[1];
                    message = message.Remove(0, 1);

                    markovChainTrainer.Train(message);
                }
            });
            outputAction?.Invoke($"Finished training.");
            return markovChainString;
        }

        public static MarkovChainString Load(string brainFile, Action<string> outputAction)
        {
            outputAction?.Invoke($"Loading '{brainFile}'.");

            var markovChainString = new MarkovChainString();
            var loadBehaviour = new LoadBehaviour(markovChainString, brainFile);
            var talkBehaviour = new TalkBehaviour(markovChainString);

            outputAction?.Invoke($"Generating sample.");
            loadBehaviour.Process();

            var sample = talkBehaviour.GenerateRandomSentence();
            outputAction?.Invoke($"Sample: '{sample}'.");
            return markovChainString;
        }

        public static void Save(string brainFile, MarkovChainString markovChain, Action<string> outputAction)
        {
            outputAction?.Invoke($"Saving '{brainFile}'.");

            var saveBehaviour = new SaveBehaviour(markovChain, brainFile);
            saveBehaviour.Process();
            outputAction?.Invoke($"Saved to {brainFile}");
        }
    }
}