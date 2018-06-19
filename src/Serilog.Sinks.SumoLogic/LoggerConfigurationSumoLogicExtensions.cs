using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.SumoLogic.Sinks;
using System.Net.Http;

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
        /// <param name="outputTemplate">Override default output template. Should not be used if overriding <see cref="ITextFormatter"/></param>
        /// <param name="handler">Override default http handler <see cref="ITextFormatter"/></param>
        /// <returns></returns>
        public static LoggerConfiguration SumoLogic(
            this LoggerSinkConfiguration loggerConfiguration,
            string endpointUrl,
            string sourceName = SumoLogicSink.DefaultSourceName,
            string sourceCategory = SumoLogicSink.DefaultSourceCategory,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchSizeLimit = SumoLogicSink.DefaultBatchSizeLimit,
            TimeSpan? period = null,
            ITextFormatter textFormatter = null,
            string outputTemplate = null,
            HttpMessageHandler handler = null)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            if (string.IsNullOrWhiteSpace(endpointUrl))
                throw new ArgumentNullException(nameof(endpointUrl));
            
            period = period ?? SumoLogicSink.DefaultPeriod;
            
            /* Order:
             * 1. Use textFormatter if set
             * 2. Use outputTemplate if set and textFormatter not set
             * 3. If neither set, use default message formatter
             */
            if (textFormatter == null)
            {
                var templateToUse = !string.IsNullOrWhiteSpace(outputTemplate)
                    ? outputTemplate
                    : SumoLogicSink.DefaultOutputTemplate;

                textFormatter = new MessageTemplateTextFormatter(templateToUse, null);
            }

            var sink = new SumoLogicSink(
                endpointUrl,
                sourceName,
                sourceCategory,
                textFormatter,
                batchSizeLimit,
                period.Value,
                handler);

            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}
