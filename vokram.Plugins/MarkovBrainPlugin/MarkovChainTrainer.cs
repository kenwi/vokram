using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
    }
}