using System;
using System.IO;
using System.Linq;
using System.Net;
using IrcDotNet;
using IrcDotNet.Collections;
using Vokram.Core.Extensions;
using Vokram.Plugins.MarkovBrainPlugin;
using System.Collections;
using System.Collections.Generic;
using Vokram.Core.Utils;

namespace Vokram.Plugins
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
        /*
        private static IEnumerable<string> RemoveIrcEvents(IEnumerable<string> messages)
        {
            return messages.Where(message => message.Length > 10 && message.Contains("<"));
        }*/

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
        /*
        private static string GetMessageText(string message)
        {
            return message.Split('>').Last().Trim();
        }

        private static string GetMessageTime(string message)
        {
            return message.Split(']').First().TrimStart('[');
        }*/

        private IEnumerable<string> GetParameters(string text)
        {
            return text.Split(' ');
        }

        /*
        public static MarkovChainString Train(Config parameters, Action<string> output)
        {
            output?.Invoke($"Initializing trainer '{parameters.TrainingFile}'");
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new MarkovChainTrainer(markovChainString);

            output?.Invoke($"Loading '{parameters.TrainingFile}'");
            var lines = File.ReadAllLines(parameters.TrainingFile);
            var numLinesFormatted = lines.Length.ToString("N0");
            output?.Invoke($"Number of lines: {numLinesFormatted}");

            output?.Invoke($"Removing IRC events from log");
            var messages = RemoveIrcEvents(lines.Skip(1));

            if(parameters.LogSections > 1)
            {
                output?.Invoke($"Splitting log into {parameters.LogSections} parts. Generating text from one part");
                messages = messages.Take(messages.Count() / (int)parameters.LogSections);
            }
            var numMessagesFormatted = messages.Count().ToString("N0");
            output?.Invoke($"Number of messages: {numMessagesFormatted}");

            var i = 0;
            var numReports = parameters.NumReports;
            var messageCount = messages.Count();
            var step = messageCount / numReports;

            output?.Invoke($"Processing messages");
            messages.ForEach(message =>
            {
                if(i++ % step == 0)
                {
                    var percentage = (float)100/ messageCount * i;
                    var percentageFormatted = percentage.ToString("0.");
                    var wordCountFormatted = markovChainTrainer.WordCount.ToString("N0");
                    var sentencesFormatted = markovChainTrainer.SentenceCount.ToString("N0");
                    var messageTimeFormatted = GetMessageTime(message);

                    output?.Invoke($"Processed: {percentageFormatted} %, {wordCountFormatted} words, {sentencesFormatted} sentences, logtime {messageTimeFormatted}");
                }
                
                var messageText = GetMessageText(message);
                markovChainTrainer.Train(messageText);
            });

            var uniqueWordsFormatted = markovChainString.Nodes.Count.ToString("N0");
            output?.Invoke($"Finished training");
            output?.Invoke($"Unique words in brain: {uniqueWordsFormatted}");

            return markovChainString;
        }*/

        public static MarkovChainString Load(Config parameters, Action<string> output)
        {
            output?.Invoke($"Loading '{parameters.Save}'");

            var markovChainString = new MarkovChainString();
            var loadBehaviour = new LoadBehaviour(markovChainString, parameters.Save);
            var talkBehaviour = new TalkBehaviour(markovChainString);

            output?.Invoke($"Generating samples");
            loadBehaviour.Process();
            Enumerable.Range(0, parameters.Samples).ForEach(i =>
            {
                var sample = talkBehaviour.GenerateRandomSentence();
                output?.Invoke($"{i}: '{sample}'");
            });
            return markovChainString;
        }

        public static void Save(Config parameters, MarkovChainString markovChain, Action<string> output)
        {
            output?.Invoke($"Saving '{parameters.Save}'");

            var saveBehaviour = new SaveBehaviour(markovChain, parameters.Save);
            saveBehaviour.Process();
            output?.Invoke($"Saved to '{parameters.Save}'");
        }
    }
}