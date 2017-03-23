using vokram.Core.Interfaces;

namespace vokram.Plugins
{
    public abstract class PluginBase : IIrcPlugin
    {
        public IIrcBot Bot { get; private set; }
        public string Name { get; private set; }

        public void Initialize(IIrcBot bot)
        {
            Bot = bot;
            Name = GetType().Name;

            Initialize();
        }

        protected abstract void Initialize();
    }
}