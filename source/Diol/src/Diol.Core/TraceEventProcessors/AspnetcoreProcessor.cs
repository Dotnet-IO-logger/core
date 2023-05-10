using Diol.Share.Features;
using Diol.Share.Features.Aspnetcores;
using Microsoft.Diagnostics.Tracing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Diol.Core.TraceEventProcessors
{
    public class AspnetcoreProcessor : IProcessor
    {
        private static string DebugCorrelationId = string.Empty;

        private readonly string loggerName = "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware";

        private readonly string eventName = "MessageJson";

        private EventPublisher eventObserver;

        public AspnetcoreProcessor(EventPublisher eventObserver)
        {
            this.eventObserver = eventObserver;
        }

        public bool CheckEventName(string eventName)
        {
            return eventName == this.eventName;
        }

        public bool CheckLoggerName(string name)
        {
            return name == this.loggerName;
        }

        public void OnCompleted()
        {
            Debug.WriteLine($"{nameof(AspnetcoreProcessor)} | {nameof(OnCompleted)}");
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine($"{nameof(AspnetcoreProcessor)} | {nameof(OnError)}");
        }

        public void OnNext(TraceEvent value)
        {
            // if you run the app via VS -> for responce's activity ids will be incorrect.
            var eventId = Convert.ToInt32(value.PayloadByName("EventId"));
            var eventName = value.PayloadByName("EventName")?.ToString();

            Debug.WriteLine($"{value.ActivityID} | {value.RelatedActivityID} | {eventName} | {eventId}");

            BaseDto be;

            if (eventId == 1)
                be = ParseRequestLog(value);
            else if (eventId == 2)
                be = ParseResponseLog(value);
            else if (eventId == 3)
                be = ParseRequestBody(value);
            else if (eventId == 4)
                be = ParseResponseBody(value);
            else
                be = null;

            // send notification
            if (be != null)
            {
                this.eventObserver.AddEvent(be);
            }
        }

        /// <summary>
        /// eventId 1 - eventName RequestLog
        /// </summary>
        /// <param name="traceEvent"></param>
        private RequestLogDto ParseRequestLog(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // Protocol: http/2
            arguments.TryGetValue("Protocol", out var protocol);
            arguments.Remove("");
            // Method: GET
            arguments.TryGetValue("Method", out var method);
            arguments.Remove("Method");
            // Scheme: https
            arguments.TryGetValue("Scheme", out var scheme);
            arguments.Remove("Scheme");
            // Host: localhost:5001
            arguments.TryGetValue("Host", out var host);
            arguments.Remove("Host");
            // Path: /WeatherForecast
            arguments.TryGetValue("Path", out var path);
            arguments.Remove("Path");

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            DebugCorrelationId = correlationId;
#endif

            // all other is headers or metadata
            // create event
            return new RequestLogDto
            {
                CorrelationId = correlationId,
                Protocol = protocol,
                Method = method,
                Scheme = scheme,
                Host = host,
                Path = path,
                Metadata = arguments
            };
        }

        /// <summary>
        /// eventId 2 - eventName ResponseLog
        /// </summary>
        /// <param name="traceEvent"></param>
        public ResponseLogDto ParseResponseLog(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // StatusCode: 200
            arguments.TryGetValue("StatusCode", out var statusCode);
            arguments.Remove("StatusCode");

            // ContentType: application/json; charset=utf-8
            // can be null
            arguments.TryGetValue("ContentType", out var contentType);
            arguments.Remove("ContentType");

            // all other is headers or metadata

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            correlationId = DebugCorrelationId;
#endif

            // create event
            return new ResponseLogDto
            {
                CorrelationId = correlationId,
                StatusCode = Convert.ToInt32(statusCode),
                ContentType = contentType,
                Metadata = arguments
            };
        }

        /// <summary>
        /// eventId 3 - eventName RequestBody
        /// </summary>
        /// <param name="traceEvent"></param>
        public RequestBodyDto ParseRequestBody(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // remove "{OriginalFormat}", because it is used for formatting
            arguments.Remove("{OriginalFormat}");

            // Body
            arguments.TryGetValue("Body", out var body);
            arguments.Remove("Body");

            // all other is headers or metadata

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            correlationId = DebugCorrelationId;
#endif

            // create event
            return new RequestBodyDto
            {
                CorrelationId = correlationId,
                BodyAsString = body,
                Metadata = arguments
            };
        }

        /// <summary>
        /// eventId 4 - eventName ResponseBody
        /// </summary>
        /// <param name="traceEvent"></param>
        public ResponseBodyDto ParseResponseBody(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // remove "{OriginalFormat}", because it is used for formatting
            arguments.Remove("{OriginalFormat}");

            // Body
            arguments.TryGetValue("Body", out var body);
            arguments.Remove("Body");

            // all other is headers or metadata

            var correlationId = traceEvent.ActivityID.ToString();

#if DEBUG
            correlationId = DebugCorrelationId;
#endif

            // create event
            return new ResponseBodyDto
            {
                CorrelationId = correlationId,
                BodyAsString = body,
                Metadata = arguments
            };
        }
    }
}
