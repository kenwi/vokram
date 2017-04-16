using IrcDotNet.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Vokram.Core.Utils;

namespace Vokram.Plugins.MarkovBrainPlugin
{
    public class MarkovChainTrainer
    {
        public readonly MarkovChainString MarkovChain = new MarkovChainString();
        public int SentenceCount { get; set; }
        public int WordCount { get; set; }

        public MarkovChainTrainer(MarkovChainString markovChain)
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
            output?.Invoke($"Initializing trainer '{parameters.TrainingFile}'");
            var markovChainString = new MarkovChainString();
            var markovChainTrainer = new MarkovChainTrainer(markovChainString);

            output?.Invoke($"Loading '{parameters.TrainingFile}'");
            var lines = File.ReadAllLines(parameters.TrainingFile);
            var numLinesFormatted = lines.Length.ToString("N0");
            output?.Invoke($"Number of lines: {numLinesFormatted}");

            output?.Invoke($"Removing IRC events from log");
            var messages = RemoveIrcEvents(lines.Skip(1));

            if (parameters.LogSections > 1)
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
                if (i++ % step == 0)
                {
                    var percentage = (float)100 / messageCount * i;
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
        }

        private static IEnumerable<string> RemoveIrcEvents(IEnumerable<string> messages)
        {
            return messages.Where(message => message.Length > 10 && message.Contains("<"));
        }

        private static string GetMessageTime(string message)
        {
            return message.Split(']').First().TrimStart('[');
        }

        private static string GetMessageText(string message)
        {
            return message.Split('>').Last().Trim();
        }
    }
}