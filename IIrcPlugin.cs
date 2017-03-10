namespace vokram
{
    public interface IIrcPlugin
    {
        void Initialize(IIrcBot bot);
        string Name { get; }
    }
}