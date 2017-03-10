using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace IrcDotNet
{
    // Represents a Markov chain of arbitrary length.
    [DebuggerDisplay("{this.nodes.Count} nodes")]
    [Serializable]
    public class MarkovChainString
    {
        private static readonly IEqualityComparer<string> comparer = EqualityComparer<string>.Default;
        private readonly Random random = new Random();

        private List<MarkovChainStringNode> nodes;
        private ReadOnlyCollection<MarkovChainStringNode> nodesReadOnly;
        public string Nick;

        internal void Train(IrcMessageEventArgs message)
        {
            throw new NotImplementedException();
        }

        public MarkovChainString()
        {
            this.nodes = new List<MarkovChainStringNode>();
            this.nodesReadOnly = new ReadOnlyCollection<MarkovChainStringNode>(this.nodes);
        }

        public List<MarkovChainStringNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        public bool Contains(string value)
        {
            var node = this.nodes.SingleOrDefault(n => comparer.Equals(n.Value, value));
            return node != null;
        }

        public IEnumerable<string> GenerateSequenceFrom(string word)
        {
            var curNode = GetNode(word);
            while (true)
            {
                if (curNode.Links.Count == 0)
                    break;
                curNode = curNode.Links[random.Next(curNode.Links.Count)];
                if (curNode.Value == null)
                    break;
                yield return curNode.Value;
            }
        }

        public IEnumerable<string> GenerateSequence()
        {
            var curNode = GetNode(default(string));
            while (true)
            {
                if (curNode.Links.Count == 0)
                    break;
                curNode = curNode.Links[random.Next(curNode.Links.Count)];
                if (curNode.Value == null)
                    break;
                yield return curNode.Value;
            }
        }

        public void Train(string fromValue, string toValue)
        {
            var fromNode = GetNode(fromValue);
            var toNode = GetNode(toValue);
            fromNode.AddLink(toNode);
        }

        private MarkovChainStringNode GetNode(string value)
        {
            var node = this.nodes.SingleOrDefault(n => n.Value == value);//comparer.Equals(n.Value, value));
            if (node == null)
            {
                node = new MarkovChainStringNode(value);
                this.nodes.Add(node);
            }
            return node;
        }
    }

    [DebuggerDisplay("{this.nodes.Count} nodes")]
    public class MarkovChain<T>
    {
        private static readonly IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

        private readonly Random random = new Random();

        private List<MarkovChainNode<T>> nodes;
        private ReadOnlyCollection<MarkovChainNode<T>> nodesReadOnly;

        public MarkovChain()
        {
            this.nodes = new List<MarkovChainNode<T>>();
            this.nodesReadOnly = new ReadOnlyCollection<MarkovChainNode<T>>(this.nodes);
        }

        public ReadOnlyCollection<MarkovChainNode<T>> Nodes
        {
            get { return nodesReadOnly; }
        }

        public IEnumerable<T> GenerateSequence()
        {
            var curNode = GetNode(default(T));
            while (true)
            {
                if (curNode.Links.Count == 0)
                    break;
                curNode = curNode.Links[random.Next(curNode.Links.Count)];
                if (curNode.Value == null)
                    break;
                yield return curNode.Value;
            }
        }

        public void Train(T fromValue, T toValue)
        {
            var fromNode = GetNode(fromValue);
            var toNode = GetNode(toValue);
            fromNode.AddLink(toNode);
        }

        private MarkovChainNode<T> GetNode(T value)
        {
            var node = this.nodes.SingleOrDefault(n => comparer.Equals(n.Value, value));
            if (node == null)
            {
                node = new MarkovChainNode<T>(value);
                this.nodes.Add(node);
            }
            return node;
        }
    }
}
