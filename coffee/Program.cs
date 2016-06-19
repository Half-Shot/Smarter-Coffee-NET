using System;
using SmarterCoffee;
namespace coffee
{
    class MainClass
    {
        //Exit codes:
        // 1 - Unknown failure
        // 2 - Bad Args
        // 3 - Failed to connect to coffee machine
        static bool ShouldRun = false;
        public static void Main (string[] args)
        {
            if(args.Length < 1){
                Console.WriteLine("Missing host/port");
                System.Environment.Exit(2);
            }

            int port = 2081;
            if(args.Length >= 2){
                if(!int.TryParse(args[1],out port)){
                    Console.WriteLine("Port is not an integer");
                    System.Environment.Exit(2);
                    return;
                }
            }

            string hostname = args[0];

            Console.WriteLine("Coffee Shell " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version);
            Console.WriteLine("Connecting to " + hostname + ":" + port);
            CoffeeClient client;
            try
            {
                client = new CoffeeClient(hostname,port);
                client.StartStatusThread();
                Console.WriteLine("Coffee Shell is connected");
            }
            catch(Exception e){
                Console.Error.WriteLine("Couldn't connect to coffee machine :(. Reason: " + e.InnerException.Message);
                System.Environment.Exit(3);
                return;
            }

            ShouldRun = true;
            while(ShouldRun){
                Console.Write(">");
                string cmd = Console.ReadLine();
            }
        }
    }
}
