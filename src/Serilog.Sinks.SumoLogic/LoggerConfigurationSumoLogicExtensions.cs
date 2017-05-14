using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.SumoLogic.Sinks;

namespace Serilog.Sinks.SumoLogic
{
    /// <summary>
    /// Adds the WriteTo.SumoLogic() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationSumoLogicExtensions
    {
        /// <summary>
        /// Adds the WriteTo.SumoLogic() extension method to <see cref="LoggerConfiguration"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration</param>
        /// <param name="endpointUrl">Sumo Logic endpoint URL to send logs to</param>
        /// <param name="sourceName">Sumo Logic source name</param>
        /// <param name="sourceCategory">Sumo Logic source category</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="textFormatter">Supplies how logs should be formatted, or null to use the default</param>
        /// <returns></returns>
        public static LoggerConfiguration SumoLogic(
            this LoggerSinkConfiguration loggerConfiguration,
            string endpointUrl,
            string sourceName = SumoLogicSink.DefaultSourceName,
            string sourceCategory = SumoLogicSink.DefaultSourceCategory,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchSizeLimit = SumoLogicSink.DefaultBatchSizeLimit,
            TimeSpan? period = null,
            ITextFormatter textFormatter = null)
        {
            if(loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            if(string.IsNullOrWhiteSpace(endpointUrl))
                throw new ArgumentNullException(nameof(endpointUrl));
            
            period = period ?? SumoLogicSink.DefaultPeriod;
            textFormatter = textFormatter ?? SumoLogicSink.DefaultTextFormatter;

            var sink = new SumoLogicSink(
                endpointUrl,
                sourceName,
                sourceCategory,
                textFormatter,
                batchSizeLimit,
                period.Value);

            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}
