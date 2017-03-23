using System;
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
            /*
            var list = new List<IIrcPlugin>();
            var domain = AppDomain.CreateDomain("Pluginsyo");
            var assemblyName = new AssemblyName();
            assemblyName.CodeBase = "vokram.Plugins.dll";
            var assembly = domain.Load(assemblyName);
            var types = assembly.GetTypes().Where(t => t.Namespace!= null && t.Namespace.Equals("vokram.Plugins") && !t.IsAbstract);
            AppDomain.Unload(domain);
            types.ForEach(t =>
            {
                var plugin = Activator.CreateInstance(t) as IIrcPlugin;
                list.Add(plugin);
            });
            return list;
            */
            var list = new List<IIrcPlugin>();
            var assembly = Assembly.LoadFile("vokram.Plugins.dll");
            var types = assembly.GetTypes().Where(t => t.Namespace!= null && t.Namespace.Equals("vokram.Plugins") && !t.IsAbstract);
            types.ForEach(t =>
            {
                var plugin = Activator.CreateInstance(t) as IIrcPlugin;
                list.Add(plugin);
            });
            return list;
        }
    }
}