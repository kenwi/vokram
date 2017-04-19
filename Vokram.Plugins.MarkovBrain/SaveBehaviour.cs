using System.IO;
using Vokram.Core.Utils;

namespace Vokram.Plugins.MarkovBrain
{
    public class SaveBehaviour
    {
        private readonly MarkovChainString _markovChainString;
        private string _filename;

        public SaveBehaviour(MarkovChainString markovChainString, string filename)
        {
            _markovChainString = markovChainString;
            _filename = filename;
        }

        public void Process()
        {
            Save(_filename);
        }

        private void Save(string filename)
        {
            filename = filename.EndsWith(".txt") ? filename : $"{filename}.txt";
            using (var fileStream = File.Create(filename))
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