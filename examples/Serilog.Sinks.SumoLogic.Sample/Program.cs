using System;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Raw;

namespace Serilog.Sinks.SumoLogic.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string url = "[YOUR_SUMO_LOGIC_URL]";

            var log = BasicUsage(url);
            //var log = CustomSourceName(url);
            //var log = CustomSourceNameAndCategory(url);
            //var log = CustomTextFormatter(url);
            //var log = OverrideEverything(url);

            log.Debug("debug message");
            log.Information("information message");
            log.Error("error message 3");
            log.Error(new Exception("doh!"), "error message with exception");

            Console.ReadLine();
        }

        private static Logger BasicUsage(string url)
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(configuration => configuration.SumoLogic(url))   // use in console apps
                .WriteTo.SumoLogic(url)       // use in ASP.NET
                .CreateLogger();
        }

        private static Logger CustomSourceName(string url)
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(configuration => 
                                configuration.SumoLogic(url, "CustomSourceName"))   // use in console apps
                .WriteTo.SumoLogic(url, "CustomSourceName")       // use in ASP.NET
                .CreateLogger();
        }

        private static Logger CustomSourceNameAndCategory(string url)
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(configuration =>
                                configuration.SumoLogic(url, "CustomSourceName", "CustomSourceCategory"))   // use in console apps
                .WriteTo.SumoLogic(url, "CustomSourceName", "CustomSourceCategory")       // use in ASP.NET
                .CreateLogger();
        }

        private static Logger CustomTextFormatter(string url)
        {
            ITextFormatter textFormatter = new RawFormatter();

            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(configuration =>
                                configuration.SumoLogic(url, "CustomTextFormatter", textFormatter: textFormatter))   // use in console apps
                .WriteTo.SumoLogic(url, "CustomTextFormatter", textFormatter: textFormatter)       // use in ASP.NET
                .CreateLogger();
        }

        private static Logger OverrideEverything(string url)
        {
            ITextFormatter textFormatter = new RawFormatter();

            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Async(configuration =>
                                configuration.SumoLogic(
                                    url, 
                                    sourceName: "CustomSourceName",
                                    sourceCategory: "CustomSourceCategory",
                                    restrictedToMinimumLevel: LogEventLevel.Debug,
                                    batchSizeLimit: 20,
                                    period: TimeSpan.FromSeconds(1),
                                    textFormatter: new RawFormatter()))     // use in console apps
                .WriteTo.SumoLogic(
                                    url,
                                    sourceName: "CustomSourceName",
                                    sourceCategory: "CustomSourceCategory",
                                    restrictedToMinimumLevel: LogEventLevel.Debug,
                                    batchSizeLimit: 20,
                                    period: TimeSpan.FromSeconds(1),
                                    textFormatter: new RawFormatter())       // use in ASP.NET
                .CreateLogger();
        }
    }
}
