using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using IrcDotNet.Collections;

namespace Vokram.Plugins.MarkovBrain
{
    using Core.Utils;
    using Plugins.MarkovBrain;

    public class Trainer
    {
        public readonly MarkovChainString MarkovChain = new MarkovChainString();
        public int SentenceCount { get; set; }
        public int WordCount { get; set; }

        public Trainer(MarkovChainString markovChain)
        {
            MarkovChain = markovChain;
        }

        public void Train(string message)
        {
            var sentences = CreateSentences(message);
            foreach (var sentence in sentences)
            {
                string lastWord = null;
                var words = CreateWords(sentence);
                foreach (var word in words)
                {
                    MarkovChain.Train(lastWord, word);
                    lastWord = word;
                }
                MarkovChain.Train(lastWord, null);
                WordCount += words.Count();
            }
            SentenceCount += sentences.Count();
            ;
        }

        public static IEnumerable<string> CreateWords(string sentence)
        {
            return sentence.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => new Regex(@"[()\[\]{}'""`~]").Replace(w, string.Empty))
                .ToArray();
        }

        public static IEnumerable<string> CreateSentences(string message)
        {
            return message
                .Split(new char[] {'.', '!', '?', ',', ';', ':'}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static MarkovChainString Train(Config parameters, Action<string> output)
        {
            Func<int, string> formatInteger = (n) => n.ToString("N0");
            Func<float, string> formatFloat = (n) => n.ToString("0.");
            Func<string, string> getMessageText = (m) => m.Split('>').Last().Trim();
            Func<string, string> getMessageNick = (m) => m.Split(' ').Skip(2).First();
            Func<string, string> getMessageTime = (m) => m.Split(']').First().TrimStart('[');
            Func<IEnumerable<string>, IEnumerable<string>> removeIrcEvents = (m) => m.Skip(1).Where(message => message.Length > 10 && message.Contains("<"));

            output?.Invoke($"Initializing trainer '{parameters.Load}'");
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new Trainer(markovChainString);

            output?.Invoke($"Loading '{parameters.Load}'");
            var lines = File.ReadAllLines(parameters.Load);
            var numLinesFormatted = formatInteger(lines.Length);
            output?.Invoke($"Number of lines: {numLinesFormatted}");

            output?.Invoke($"Removing IRC events from log");
            var messages = removeIrcEvents(lines);

            if (parameters.Sections > 1)
            {
                output?.Invoke($"Splitting log into {parameters.Sections} parts. Generating text from one part");
                messages = messages.Reverse().Take(messages.Count() / (int)parameters.Sections);
            }
            var numMessagesFormatted = formatInteger(messages.Count());
            output?.Invoke($"Number of messages: {numMessagesFormatted}");

            if (parameters.Filter != "")
            {
                output?.Invoke($"Applying filter '{parameters.Filter}'");

                messages = messages.Where(message => {
                    var text = getMessageText(message);
                    var nick = getMessageNick(message);
                    return Regex.IsMatch(text, parameters.Filter, RegexOptions.IgnoreCase) || Regex.IsMatch(nick, parameters.Filter, RegexOptions.IgnoreCase);
                });

                numMessagesFormatted = formatInteger(messages.Count());
                output?.Invoke($"Number of messages: {numMessagesFormatted}");
            }

            var i = 0;
            var numReports = parameters.Reports;
            var messageCount = messages.Count();
            var step = messageCount / numReports;

            output?.Invoke($"Processing messages");
            messages.ForEach(message =>
            {
                if (i++ % step == 0)
                {
                    var percentage = (float)100 / messageCount * i;
                    var percentageFormatted = formatFloat(percentage);
                    var wordCountFormatted = formatInteger(markovChainTrainer.WordCount);
                    var sentencesFormatted = formatInteger(markovChainTrainer.SentenceCount);
                    var messageTimeFormatted = getMessageTime(message);

                    output?.Invoke($"Processed: {percentageFormatted} %, {wordCountFormatted} words, {sentencesFormatted} sentences, logtime {messageTimeFormatted}");
                }

                var messageText = getMessageText(message);
                markovChainTrainer.Train(messageText);
            });

            var uniqueWordsFormatted = formatInteger(markovChainString.Nodes.Count);
            output?.Invoke($"Finished training");
            output?.Invoke($"Unique words in brain: {uniqueWordsFormatted}");

            return markovChainString;
        }

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