using System;
using System.Threading.Tasks;
using IMAP_Server.CommandModels;
using Serilog;
using Serilog.Sinks.SystemConsole;

namespace IMAP_Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Log.Logger = log;
                
            Server server = new Server("127.0.0.1",143);
            await server.StartListening();

        }
    }
}
