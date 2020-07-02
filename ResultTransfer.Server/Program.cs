using System;

namespace ResultTransfer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleWebServer simpleWebServer = new SimpleWebServer();
            simpleWebServer.Run();
            Console.ReadKey();
        }
    }
}
