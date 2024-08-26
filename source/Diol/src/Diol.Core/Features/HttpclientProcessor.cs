using Diol.Core.TraceEventProcessors;
using Diol.Share.Features;
using Diol.Share.Features.Httpclients;
using Microsoft.Diagnostics.Tracing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diol.Core.Features
{
    /// <summary>
    /// Represents a processor for HttpClient events.
    /// </summary>
    public class HttpClientProcessor : BaseProcessor
    {
        private readonly string loggerNameBegin = "System.Net.Http.HttpClient";

        private readonly string loggerNameEnd = "LogicalHandler";

        private readonly string eventName = "MessageJson";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientProcessor"/> class.
        /// </summary>
        /// <param name="eventObserver">The event observer.</param>
        public HttpClientProcessor(EventPublisher eventObserver)
            : base(eventObserver)
        {
        }

        private bool CheckEventName(string eventName) =>
            eventName == this.eventName;

        private bool CheckLoggerName(string name) =>
            name.StartsWith(loggerNameBegin)
                && name.EndsWith(loggerNameEnd);

        /// <summary>
        /// Checks if the logger name and event name match the expected values.
        /// </summary>
        /// <param name="loggerName">The logger name.</param>
        /// <param name="eventName">The event name.</param>
        /// <returns><c>true</c> if the logger name and event name match the expected values; otherwise, <c>false</c>.</returns>
        public override bool CheckEvent(string loggerName, string eventName) =>
            CheckLoggerName(loggerName)
            && CheckEventName(eventName);

        /// <summary>
        /// Gets the log DTO based on the event ID and trace event.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="value">The trace event.</param>
        /// <returns>The log DTO.</returns>
        public override BaseDto GetLogDto(int eventId, TraceEvent value)
        {
            if (eventId == 100)
                return ParseRequestPipelineStart(value);
            else if (eventId == 101)
                return ParseRequestPipelineEnd(value);
            else if (eventId == 102)
                return ParseRequestPipelineRequestHeader(value);
            else if (eventId == 103)
                return ParseRequestPipelineResponseHeader(value);
            else
                return null;
        }

        /// <summary>
        /// Parses the RequestPipelineStart event and returns the corresponding DTO.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The RequestPipelineStart DTO.</returns>
        public static RequestPipelineStartDto ParseRequestPipelineStart(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);
            var httpMethod = arguments["HttpMethod"];
            var uri = arguments["Uri"];

            var correlationId = traceEvent.ActivityID.ToString();

            return new RequestPipelineStartDto
            {
                CorrelationId = correlationId,
                HttpMethod = httpMethod,
                Uri = uri
            };
        }

        /// <summary>
        /// Parses the RequestPipelineRequestHeader event and returns the corresponding DTO.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The RequestPipelineRequestHeader DTO.</returns>
        public static RequestPipelineRequestHeaderDto ParseRequestPipelineRequestHeader(TraceEvent traceEvent)
        {
            var headersAsText = traceEvent.PayloadByName("FormattedMessage")?.ToString();

            var result = ParseHeaders(headersAsText);

            var correlationId = traceEvent.ActivityID.ToString();

            return new RequestPipelineRequestHeaderDto
            {
                CorrelationId = correlationId,
                Headers = result
            };
        }

        /// <summary>
        /// Parses the RequestPipelineEnd event and returns the corresponding DTO.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The RequestPipelineEnd DTO.</returns>
        public static RequestPipelineEndDto ParseRequestPipelineEnd(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);
            var elapsedMilliseconds = arguments["ElapsedMilliseconds"];
            var statusCode = arguments["StatusCode"];

            var correlationId = traceEvent.ActivityID.ToString();

            // parse from millisecond to TimeSpan
            return new RequestPipelineEndDto
            {
                CorrelationId = correlationId,
                ElapsedMilliseconds = TimeSpan.FromMilliseconds(double.Parse(elapsedMilliseconds.ToString())),
                StatusCode = int.Parse(statusCode)
            };
        }

        /// <summary>
        /// Parses the RequestPipelineResponseHeader event and returns the corresponding DTO.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The RequestPipelineResponseHeader DTO.</returns>
        public static RequestPipelineResponseHeaderDto ParseRequestPipelineResponseHeader(TraceEvent traceEvent)
        {
            var headersAsText = traceEvent.PayloadByName("FormattedMessage")?.ToString();

            var result = ParseHeaders(headersAsText);

            var correlationId = traceEvent.ActivityID.ToString();

            return new RequestPipelineResponseHeaderDto
            {
                CorrelationId = correlationId,
                Headers = result
            };
        }

        private static Dictionary<string, string> ParseHeaders(string headersAsText)
        {
            var headers = headersAsText.Split('\n').Skip(1);
            var headersDictionary = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                // TODO: use .split(':') instead of IndexOf and Substring
                var deviderIndex = header.IndexOf(':');
                if (deviderIndex == -1)
                {
                    continue;
                }
                var key = header.Substring(0, deviderIndex);
                var value = header.Substring(deviderIndex + 1);
                headersDictionary.Add(key, value);
            }
            return headersDictionary;
        }
    }
}
