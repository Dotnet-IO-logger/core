using Diol.Share.Features;
using Diol.Share.Features.Httpclients;
using Microsoft.Diagnostics.Tracing;
using System.Diagnostics;
using System.Text.Json;

namespace Diol.Core.TraceEventProcessors
{
    public class HttpclientProcessor : IProcessor
    {
        private readonly string loggerNameBegin = "System.Net.Http.HttpClient";

        private readonly string loggerNameEnd = "LogicalHandler";

        private readonly string eventName = "MessageJson";

        private EventPublisher eventObserver;

        public HttpclientProcessor(EventPublisher eventObserver)
        {
            this.eventObserver = eventObserver;
        }

        public bool CheckEventName(string eventName)
        {
            return eventName == this.eventName;
        }

        public bool CheckLoggerName(string name)
        {
            return name.StartsWith(this.loggerNameBegin) 
                && name.EndsWith(this.loggerNameEnd);
        }

        public void OnCompleted()
        {
            Debug.WriteLine($"{nameof(HttpclientProcessor)} | {nameof(OnCompleted)}");
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine($"{nameof(HttpclientProcessor)} | {nameof(OnError)}");
        }

        public void OnNext(TraceEvent value)
        {
            var eventId = Convert.ToInt32(value.PayloadByName("EventId"));
            var eventName = value.PayloadByName("EventName")?.ToString();

            // ActivityId : Guid? 
            // traceEvent.ActivityID - is correlation id.
            // IMPORTANT:
            // in debug mode, ActivityID will be not correct for eventId 101 and 103
            Debug.WriteLine($"{value.ActivityID} | {eventName}");

            BaseDto be;

            if (eventId == 100)
                be = ParseRequestPipelineStart(value);
            else if (eventId == 101)
                be = ParseRequestPipelineEnd(value);
            else if (eventId == 102)
                be = ParseRequestPipelineRequestHeader(value);
            else if (eventId == 103)
                be = ParseRequestPipelineResponseHeader(value);
            else
                be = null;

            // send notification
            if (be != null)
            {
                this.eventObserver.AddEvent(be);
            }
        }

        /// <summary>
        /// eventId 100 - eventName RequestPipelineStart
        /// </summary>
        /// <param name="traceEvent"></param>
        public static RequestPipelineStartDto ParseRequestPipelineStart(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonSerializer.Deserialize<Dictionary<string, string>>(argumentsAsJson);
            var httpMethod = arguments["HttpMethod"];
            var uri = arguments["Uri"];

            return new RequestPipelineStartDto
            {
                CorrelationId = traceEvent.ActivityID.ToString(),
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

            return new RequestPipelineRequestHeaderDto
            {
                CorrelationId = traceEvent.ActivityID.ToString(),
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
            var arguments = JsonSerializer.Deserialize<Dictionary<string, string>>(argumentsAsJson);
            var elapsedMilliseconds = arguments["ElapsedMilliseconds"];
            var statusCode = arguments["StatusCode"];

            // parse from milisecond to TimeSpan
            return new RequestPipelineEndDto
            {
                CorrelationId = traceEvent.ActivityID.ToString(),
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

            return new RequestPipelineResponseHeaderDto
            {
                CorrelationId = traceEvent.ActivityID.ToString(),
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
