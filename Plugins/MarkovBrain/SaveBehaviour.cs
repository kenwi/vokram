using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using IrcDotNet;
using vokram.Utils;

namespace vokram
{
    public class SaveBehaviour
    {
        public string BrainFile { get; set; }
        private readonly MarkovChainString _markovChainString;

        public SaveBehaviour(MarkovChainString markovChainString)
        {
            _markovChainString = markovChainString;
        }

        public void Process()
        {
            Save(BrainFile);
        }

        private void Save(string brainFile)
        {
            brainFile = $"{brainFile}.txt";
            using (var fileStream = File.Create(brainFile + ".txt"))
            {
                var memoryStream = Serializer.SerializeToStream(_markovChainString);
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(fileStream);
                fileStream.Flush();
            }
        }

/*
        public static MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }
    }*/
    }
}