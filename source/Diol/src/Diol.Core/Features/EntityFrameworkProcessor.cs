using Diol.Core.TraceEventProcessors;
using Diol.Core.Utils;
using Diol.Share.Features;
using Diol.Share.Features.EntityFrameworks;
using Microsoft.Diagnostics.Tracing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Diol.Core.Features
{
    /// <summary>
    /// Represents a processor for Entity Framework events.
    /// </summary>
    public class EntityFrameworkProcessor : BaseProcessor
    {
        private readonly string loggerNameBegin = "Microsoft.EntityFrameworkCore.Database";
        private readonly string loggerNameEndConnection = "Connection";
        private readonly string loggerNameEndCommand = "Command";

        private readonly string eventName = "MessageJson";

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkProcessor"/> class.
        /// </summary>
        /// <param name="eventObserver">The event observer.</param>
        public EntityFrameworkProcessor(EventPublisher eventObserver)
            : base(eventObserver)
        {
        }

        private bool CheckEventName(string eventName) =>
            eventName == this.eventName;

        private bool CheckLoggerName(string name) =>
            name.StartsWith(loggerNameBegin)
            && (name.EndsWith(loggerNameEndConnection)
                || name.EndsWith(loggerNameEndCommand));

        /// <summary>
        /// Checks if the event matches the logger name and event name for Entity Framework events.
        /// </summary>
        /// <param name="loggerName">The logger name.</param>
        /// <param name="eventName">The event name.</param>
        /// <returns><c>true</c> if the event matches the logger name and event name; otherwise, <c>false</c>.</returns>
        public override bool CheckEvent(string loggerName, string eventName) =>
            CheckLoggerName(loggerName)
            && CheckEventName(eventName);

        /// <summary>
        /// Gets the log DTO for the specified event ID and trace event.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="value">The trace event.</param>
        /// <returns>The log DTO.</returns>
        public override BaseDto GetLogDto(int eventId, TraceEvent value)
        {
            if (eventId == 20000)
                return ConnectionOpening(value);
            else if (eventId == 20100)
                return CommandExecuting(value);
            else if (eventId == 20101)
                return CommandExecuted(value);
            else
                return null;
        }

        private ConnectionOpeningDto ConnectionOpening(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            var correlationId = traceEvent.ActivityID.ToString();

            arguments.TryGetValueAndRemove("database", out string database);
            arguments.TryGetValueAndRemove("server", out string server);

            return new ConnectionOpeningDto()
            {
                CorrelationId = correlationId,
                Database = database,
                Server = server
            };
        }

        private CommandExecutingDto CommandExecuting(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            var correlationId = traceEvent.ActivityID.ToString();

            arguments.TryGetValueAndRemove("parameters", out string parameters);
            arguments.TryGetValueAndRemove("commandText", out string commandText);

            return new CommandExecutingDto()
            {
                CorrelationId = correlationId,
                Parameters = parameters,
                CommandText = commandText
            };
        }

        private CommandExecutedDto CommandExecuted(TraceEvent traceEvent)
        {
            var argumentsAsJson = traceEvent.PayloadByName("ArgumentsJson")?.ToString();
            var arguments = JsonConvert.DeserializeObject<Dictionary<string, string>>(argumentsAsJson);

            arguments.TryGetValueAndRemove("elapsed", out string elapsed);
            var elapsedMs = TimeSpan.FromMilliseconds(double.Parse(elapsed));

            var correlationId = traceEvent.ActivityID.ToString();

            return new CommandExecutedDto()
            {
                CorrelationId = correlationId,
                ElapsedMilliseconds = elapsedMs
            };
        }
    }
}
