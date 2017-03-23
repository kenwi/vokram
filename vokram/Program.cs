namespace vokram
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var vokram = new VokramBot("irc.freenode.net", "vokram2"))
            {
                vokram.ClientRegistered = (sender, eventArgs) => vokram.Join("#hadamard");
                vokram.ConnectAndEnterMainLoop();
            }
        }

    }
}