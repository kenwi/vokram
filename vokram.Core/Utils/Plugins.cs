using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IrcDotNet.Collections;
using vokram.Core.Interfaces;

namespace vokram.Core.Utils
{
    public static class Plugins
    {
        public static List<IIrcPlugin> LoadAll()
        {
            var list = new List<IIrcPlugin>();
            var assembly = Assembly.LoadFile("vokram.Plugins.dll");
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(IIrcPlugin)));
            types.ForEach(t =>
            {
                var plugin = assembly.CreateInstance(t.FullName) as IIrcPlugin;
                list.Add(plugin);
            });
            return list;
        }
    }
}