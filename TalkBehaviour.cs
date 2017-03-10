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
            return string.Join(" ", words) + ".";
        }
    }
}