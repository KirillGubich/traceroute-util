using System;

namespace traceroute_util
{
    class Program
    {
        static void Main(string[] args)
        {
            launchMenu();
        }

        private static void launchMenu()
        {
            string userInput = "";
            while (userInput != "exit")
            {
                Console.Write(">> ");
                userInput = Console.ReadLine();
                string hostName;
                bool viewDns = false;
                if (userInput != null && userInput.IndexOf("tracert") == 0)
                {
                    userInput = userInput.Remove(0, 8);
                    if (userInput.IndexOf("-d", StringComparison.Ordinal) > 0)
                    {
                        userInput = userInput.Remove(userInput.IndexOf("-d", StringComparison.Ordinal) - 1);
                        viewDns = true;
                    }
                    hostName = userInput;
                    Traceroute tracerouteUtil = Traceroute.getInstance();
                    tracerouteUtil.Run(hostName, viewDns);
                }
                else if (!userInput.Equals("exit"))
                {
                    Console.WriteLine("Некорректные данные.");
                }
            }
        }
    }  
}
