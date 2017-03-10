using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using vokram.Utils;

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
                using (var fileStream = File.OpenRead(filename))
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
                throw new Exception($"Could not load brain '{filename}'");
            }
        }
    }
}