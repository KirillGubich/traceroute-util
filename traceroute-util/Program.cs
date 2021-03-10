using System;

namespace traceroute_util
{
    class Program
    {
        private static Traceroute traceroute = new Traceroute();

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                launchMenu(args[0], args[1]);
            }
        }

        private static void launchMenu(string hostName, string param)
        {
            bool viewDns = param.Equals("-d");
            traceroute.Run(hostName, viewDns);
        }
    }  
}
