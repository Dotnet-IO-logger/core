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
    public class HttpclientProcessor : BaseProcessor
    {
        private static string DebugCorrelationId = string.Empty;

        private readonly string loggerNameBegin = "System.Net.Http.HttpClient";

        private readonly string loggerNameEnd = "LogicalHandler";

        private readonly string eventName = "MessageJson";

        public HttpclientProcessor(EventPublisher eventObserver)
            : base(eventObserver)
        {
        }

        private bool CheckEventName(string eventName) =>
            eventName == this.eventName;

        private bool CheckLoggerName(string name) =>
            name.StartsWith(loggerNameBegin)
                && name.EndsWith(loggerNameEnd);

        public override bool CheckEvent(string loggerName, string eventName) =>
            CheckLoggerName(loggerName)
            && CheckEventName(eventName);

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
        /// eventId 100 - eventName RequestPipelineStart
        /// </summary>
        /// <param name="traceEvent"></param>
        public static RequestPipelineStartDto ParseRequestPipelineStart(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);
            var httpMethod = arguments["HttpMethod"];
            var uri = arguments["Uri"];

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            //DebugCorrelationId = correlationId;
#endif

            return new RequestPipelineStartDto
            {
                CorrelationId = correlationId,
                HttpMethod = httpMethod,
                Uri = uri
            };
        }

        /// <summary>
        /// eventId 102 - eventName RequestPipelineRequestHeader
        /// </summary>
        /// <param name="traceEvent"></param>
        public static RequestPipelineRequestHeaderDto ParseRequestPipelineRequestHeader(TraceEvent traceEvent)
        {
            var headersAsText = traceEvent.PayloadByName("FormattedMessage")?.ToString();

            var result = ParseHeaders(headersAsText);

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            //correlationId = DebugCorrelationId;
#endif

            return new RequestPipelineRequestHeaderDto
            {
                CorrelationId = correlationId,
                Headers = result
            };
        }

        /// <summary>
        /// eventId 101 - eventName RequestPipelineEnd
        /// </summary>
        /// <param name="traceEvent"></param>
        public static RequestPipelineEndDto ParseRequestPipelineEnd(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);
            var elapsedMilliseconds = arguments["ElapsedMilliseconds"];
            var statusCode = arguments["StatusCode"];

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            //correlationId = DebugCorrelationId;
#endif

            // parse from milisecond to TimeSpan
            return new RequestPipelineEndDto
            {
                CorrelationId = correlationId,
                ElapsedMilliseconds = TimeSpan.FromMilliseconds(double.Parse(elapsedMilliseconds.ToString())),
                StatusCode = int.Parse(statusCode)
            };
        }

        /// <summary>
        /// eventId 103 - eventName RequestPipelineResponseHeader
        /// </summary>
        /// <param name="traceEvent"></param>
        public static RequestPipelineResponseHeaderDto ParseRequestPipelineResponseHeader(TraceEvent traceEvent)
        {
            var headersAsText = traceEvent.PayloadByName("FormattedMessage")?.ToString();

            var result = ParseHeaders(headersAsText);

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            //correlationId = DebugCorrelationId;
#endif

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
