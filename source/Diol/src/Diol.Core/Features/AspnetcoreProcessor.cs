using Diol.Core.TraceEventProcessors;
using Diol.Core.Utils;
using Diol.Share.Features;
using Diol.Share.Features.Aspnetcores;
using Microsoft.Diagnostics.Tracing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Diol.Core.Features
{
    /// <summary>
    /// Processor for ASP.NET Core events.
    /// </summary>
    public class AspnetcoreProcessor : BaseProcessor
    {
        private const string LoggerName = "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware";

        private const string EventName = "MessageJson";

        public AspnetcoreProcessor(EventPublisher eventObserver)
            : base(eventObserver)
        {
        }

        /// <summary>
        /// Checks if the event matches the logger name and event name.
        /// </summary>
        /// <param name="loggerName">The logger name.</param>
        /// <param name="eventName">The event name.</param>
        /// <returns>True if the event matches, otherwise false.</returns>
        public override bool CheckEvent(string loggerName, string eventName) =>
            LoggerName == loggerName
            && EventName == eventName;

        /// <summary>
        /// Gets the corresponding log DTO based on the event ID.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="value">The trace event.</param>
        /// <returns>The log DTO.</returns>
        public override BaseDto GetLogDto(int eventId, TraceEvent value)
        {
            if (eventId == 1)
                return ParseRequestLog(value);
            else if (eventId == 2)
                return ParseResponseLog(value);
            else if (eventId == 3)
                return ParseRequestBody(value);
            else if (eventId == 4)
                return ParseResponseBody(value);
            else
                return null;
        }

        /// <summary>
        /// Parses the trace event to a RequestLogDto.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The RequestLogDto.</returns>
        private RequestLogDto ParseRequestLog(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // Protocol: http/2
            arguments.TryGetValueAndRemove("Protocol", out var protocol);
            // Method: GET
            arguments.TryGetValueAndRemove("Method", out var method);
            // Scheme: https
            arguments.TryGetValueAndRemove("Scheme", out var scheme);
            // Host: localhost:5001
            arguments.TryGetValueAndRemove("Host", out var host);
            // Path: /WeatherForecast
            arguments.TryGetValueAndRemove("Path", out var path);

            var correlationId = traceEvent.ActivityID.ToString();

            var uri = $"{scheme}://{host}{path}";

            var queryParameters = Utilities.GetQueryParams(uri);

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
                Metadata = arguments,
                QueryParameters = queryParameters,
                Uri = uri
            };
        }

        /// <summary>
        /// Parses the trace event to a ResponseLogDto.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The ResponseLogDto.</returns>
        public ResponseLogDto ParseResponseLog(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // StatusCode: 200
            arguments.TryGetValueAndRemove("StatusCode", out var statusCode);

            // ContentType: application/json; charset=utf-8
            // can be null
            arguments.TryGetValueAndRemove("ContentType", out var contentType);

            // all other is headers or metadata

            var correlationId = traceEvent.ActivityID.ToString();

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
        /// Parses the trace event to a RequestBodyDto.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The RequestBodyDto.</returns>
        public RequestBodyDto ParseRequestBody(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // remove "{OriginalFormat}", because it is used for formatting
            arguments.Remove("{OriginalFormat}");

            // Body
            arguments.TryGetValueAndRemove("Body", out var body);

            // all other is headers or metadata

            var correlationId = traceEvent.ActivityID.ToString();

            // create event
            return new RequestBodyDto
            {
                CorrelationId = correlationId,
                BodyAsString = body,
                Metadata = arguments
            };
        }

        /// <summary>
        /// Parses the trace event to a ResponseBodyDto.
        /// </summary>
        /// <param name="traceEvent">The trace event.</param>
        /// <returns>The ResponseBodyDto.</returns>
        public ResponseBodyDto ParseResponseBody(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            // remove "{OriginalFormat}", because it is used for formatting
            arguments.Remove("{OriginalFormat}");

            // Body
            arguments.TryGetValueAndRemove("Body", out var body);

            // all other is headers or metadata

            var correlationId = traceEvent.ActivityID.ToString();

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
