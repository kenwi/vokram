using System;
using System.ComponentModel;

namespace Vokram.Core.Utils
{
    public class Config
    {
        public string TrainingFile { get; set; }
        public string BrainFile { get; set; }
        public int LogSections { get; set; }
        public int NumReports { get; set; }
        public int NumSamples { get; set; }

        public override string ToString()
        {
            var output = "";
            foreach(PropertyDescriptor descriptor in  TypeDescriptor.GetProperties(this))
            {
                output += $"{descriptor.Name}={descriptor.GetValue(this)}, ";
            }
            return output.TrimEnd(' ').TrimEnd(',');
        }
    }
}
