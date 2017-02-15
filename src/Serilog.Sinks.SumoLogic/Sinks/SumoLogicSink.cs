using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.SumoLogic.Sinks
{
    /// <summary>
    /// Sink for sending logs to Sumo Logic
    /// </summary>
    public class SumoLogicSink : PeriodicBatchingSink
    {
        private readonly string _endpointUrl;
        private readonly string _sourceName;
        private readonly string _sourceHost;
        private readonly ITextFormatter _textFormatter;
        private readonly HttpClient _httpClient;

        private const string SumoSourceNameRequestHeader = "X-Sumo-Name";
        private const string SumoSourceHostRequestHeader = "X-Sumo-Host";

        /// <summary>
        /// The default maximum number of events to include in a single batch.
        /// </summary>
        public const int DefaultBatchSizeLimit = 10;

        /// <summary>
        /// Sumo Logic default source name
        /// </summary>
        public const string DefaultSourceName = "Serilog";

        /// <summary>
        /// Sumo Logic default source host
        /// </summary>
        public const string DefaultSourceHost = "";

        /// <summary>
        /// The default output template
        /// </summary>
        public const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

        /// <summary>
        /// The default period.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Sink for sending logs to Sumo Logic
        /// </summary>
        /// <param name="endpointUrl">Sumo Logic endpoint URL to send logs to</param>
        /// <param name="sourceName">Sumo Logic source name</param>
        /// <param name="sourceHost">Sumo Logic source host</param>
        /// <param name="textFormatter">Supplies how logs should be formatted</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        public SumoLogicSink(
            string endpointUrl,
            string sourceName,
            string sourceHost,
            ITextFormatter textFormatter,
            int batchSizeLimit, 
            TimeSpan period) : base(batchSizeLimit, period)
        {
            _endpointUrl = endpointUrl;
            _sourceName = sourceName;
            _sourceHost = sourceHost;
            _textFormatter = textFormatter;

            _httpClient = new HttpClient();
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var tasks = events.Select(GetStringContent)
                .Select(content => _httpClient.PostAsync(_endpointUrl, content));

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            _httpClient?.Dispose();
        }

        protected string GetFormattedLog(LogEvent logEvent)
        {
            if(logEvent == null)
                throw new ArgumentNullException(nameof(logEvent));

            using (var stringWriter = new StringWriter())
            {
                _textFormatter.Format(logEvent, stringWriter);
                return stringWriter.ToString();
            }
        }

        protected StringContent GetStringContent(LogEvent logEvent)
        {
            var formattedLog = GetFormattedLog(logEvent);
            var content = new StringContent(formattedLog, Encoding.UTF8, "text/plain");
            content.Headers.Add(SumoSourceNameRequestHeader, _sourceName);
            
            if(!string.IsNullOrWhiteSpace(_sourceHost))
                content.Headers.Add(SumoSourceHostRequestHeader, _sourceHost);

            return content;
        }
    }
}
