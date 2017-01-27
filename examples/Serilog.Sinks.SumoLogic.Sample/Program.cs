using System;

namespace Serilog.Sinks.SumoLogic.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string url = "[YOUR_SUMO_LOGIC_URL]";

            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(configuration => configuration.SumoLogic(url))   // use in console apps
                //.WriteTo.SumoLogic(url)       // use in ASP.NET
                .CreateLogger();

            log.Debug("debug message");
            log.Information("information message");
            log.Error("error message 3");
            log.Error(new Exception("doh!"), "error message with exception");

            Console.ReadLine();
        }
    }
}
