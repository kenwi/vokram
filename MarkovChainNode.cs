using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace IrcDotNet
{
    [DebuggerDisplay("Value: {this.value == null ? \"(null)\" : this.value.ToString()}, {this.links.Count} links")]
    [Serializable]
    public class MarkovChainStringNode
    {
        private string value;
        private List<MarkovChainStringNode> links;
        private ReadOnlyCollection<MarkovChainStringNode> linksReadOnly;

        public MarkovChainStringNode(string value)
            : this()
        {
            this.value = value;
        }

        public MarkovChainStringNode()
        {
            this.links = new List<MarkovChainStringNode>();
            this.linksReadOnly = new ReadOnlyCollection<MarkovChainStringNode>(this.links);
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public ReadOnlyCollection<MarkovChainStringNode> Links
        {
            get { return linksReadOnly; }
        }

        public void AddLink(MarkovChainStringNode toNode)
        {
            this.links.Add(toNode);
        }
    }

    // Represents a node within a Markov chain.
    [DebuggerDisplay("Value: {this.value == null ? \"(null)\" : this.value.ToString()}, {this.links.Count} links")]
    public class MarkovChainNode<T>
    {
        private T value;
        private List<MarkovChainNode<T>> links;
        private ReadOnlyCollection<MarkovChainNode<T>> linksReadOnly;

        public MarkovChainNode(T value)
            : this()
        {
            this.value = value;
        }

        public MarkovChainNode()
        {
            this.links = new List<MarkovChainNode<T>>();
            this.linksReadOnly = new ReadOnlyCollection<MarkovChainNode<T>>(this.links);
        }

        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public ReadOnlyCollection<MarkovChainNode<T>> Links
        {
            get { return linksReadOnly; }
        }

        public void AddLink(MarkovChainNode<T> toNode)
        {
            this.links.Add(toNode);
        }
    }
}
