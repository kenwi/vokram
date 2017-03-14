using System.Linq;

namespace IrcDotNet
{
    public class TalkBehaviour
    {
        private readonly MarkovChainString _markovChainString;
        public string Sentence => GenerateRandomSentence();

        public TalkBehaviour(MarkovChainString markovChainString)
        {
            _markovChainString = markovChainString;
        }

        public string GenerateRandomSentence()
        {
            int trials = 0;
            string[] words;
            do
            {
                words = _markovChainString.GenerateSequence().ToArray();
            } while (words.Length < 3 && trials++ < 50);
            
            return WordsToSentence(words);
        }

        public string GenerateRandomSentenceFrom(string text)
        {
            int trials = 0;
            string[] words;
            do
            {
                words = _markovChainString.GenerateSequenceFrom(text).ToArray();
            } while (words.Length < 3 && trials++ < 50);

            return WordsToSentence(words);
        }

        private static string WordsToSentence(string[] words)
        {
            var sentence = string.Join(" ", words) + ".";
            sentence = FirstCharToUpper(sentence);
            return sentence;
        }

        private static string FirstCharToUpper(string text)
        {
            return text.First().ToString().ToUpper() + text.Substring(1);
        }
    }
}