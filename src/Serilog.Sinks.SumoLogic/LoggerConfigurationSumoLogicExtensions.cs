using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Display;
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
        /// <param name="sourceHost">Sumo Logic source host</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="outputTemplate">A message template describing the format used to write to the sink.
        /// the default is "{Timestamp} [{Level}] {Message}{NewLine}{Exception}".</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns></returns>
        public static LoggerConfiguration SumoLogic(
            this LoggerSinkConfiguration loggerConfiguration,
            string endpointUrl,
            string sourceName = SumoLogicSink.DefaultSourceName,
            string sourceHost = SumoLogicSink.DefaultSourceHost,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchSizeLimit = SumoLogicSink.DefaultBatchSizeLimit,
            TimeSpan? period = null,
            string outputTemplate = SumoLogicSink.DefaultOutputTemplate,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            if (string.IsNullOrEmpty(endpointUrl))
                throw new ArgumentNullException(nameof(endpointUrl));

            var defaultPeriod = period ?? SumoLogicSink.DefaultPeriod;
            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);

            var sink = new SumoLogicSink(
                endpointUrl,
                sourceName,
                sourceHost,
                formatter,
                batchSizeLimit,
                defaultPeriod);

            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}
