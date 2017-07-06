using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Display;
using Serilog.Sinks.SumoLogic;

namespace AppSettingsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var logger = new LoggerConfiguration()
                .WriteTo.SumoLogic("http://localhost", textFormatter: new MessageTemplateTextFormatter("FOOBAR", null))
                .CreateLogger();

            // From appsettings.json
            var loggerFromConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            loggerFromConfig.Information("Hello, world!");

            Console.WriteLine("Hello World!");
        }
    }
}