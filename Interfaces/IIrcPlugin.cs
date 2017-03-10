namespace vokram
{
    public interface IIrcPlugin
    {
        IIrcBot Bot { get; }
        string Name { get; }
        void Initialize(IIrcBot bot);
    }
}