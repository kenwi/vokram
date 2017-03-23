using System;
using System.IO;
using vokram.Core.Utils;

namespace IrcDotNet
{
    public class LoadBehaviour
    {
        private string _filename;
        private readonly MarkovChainString _markovChainString;

        public LoadBehaviour(MarkovChainString markovChainString, string filename)
        {
            _markovChainString = markovChainString;
            _filename = filename;
        }

        public void Process()
        {
            Load(_filename);
        }

        private void Load(string filename)
        {
            filename = filename.EndsWith(".txt") ? filename : $"{filename}.txt";
            try
            {
                using (var ms = new MemoryStream())
                using (var fileStream = File.OpenRead(Path.Combine(Environment.CurrentDirectory, filename)))
                {
                    var bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int)fileStream.Length);
                    ms.Write(bytes, 0, (int)fileStream.Length);

                    var brain = Serializer.DeserializeFromStream(ms) as MarkovChainString;
                    if (brain != null)
                    {
                        _markovChainString.Nick = brain.Nick;
                        _markovChainString.Nodes = brain.Nodes;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Could not load brain '{filename}'");
            }
        }
    }
}