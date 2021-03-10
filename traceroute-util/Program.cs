namespace traceroute_util
{
    class Program
    {
        private static Traceroute traceroute = new Traceroute();
        private const string DNS_VIEW_PARAM = "-d";

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                traceroute.Run(args[0], false);
            }
            if (args.Length > 1)
            {
                traceroute.Run(args[0], args[1].Equals(DNS_VIEW_PARAM));
            }
        }
    }  
}
