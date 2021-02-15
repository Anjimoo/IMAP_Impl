using System;
using Serilog;
using Serilog.Sinks.SystemConsole;

namespace IMAP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;
                
            Server server = new Server();
            server.StartListening();

        }
    }
}
