using System;
using System.Linq;
using System.ComponentModel;

namespace Vokram.Core.Utils
{
    public class Config
    {
        public string Load { get; set; }
        public string Save { get; set; }
        public int Sections { get; set; }
        public int Reports { get; set; }
        public int Samples { get; set; }
        public string Filter { get; set; }

        public override string ToString()
        {
            var output = "";

            foreach(PropertyDescriptor descriptor in  TypeDescriptor.GetProperties(this))
            {
                output += $"{descriptor.Name}={descriptor.GetValue(this)}, ";
            }
            return output.TrimEnd( new char [] { ',', ' ' });
        }
    }
}
