using Diol.Core.TraceEventProcessors;
using Diol.Share.Features;
using Diol.Share.Features.EntityFrameworks;
using Microsoft.Diagnostics.Tracing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Diol.Core.Features
{
    public class EntityFrameworkProcessor : BaseProcessor
    {
        private readonly string loggerNameBegin = "Microsoft.EntityFrameworkCore.Database";
        private readonly string loggerNameEndConnection = "Connection";
        private readonly string loggerNameEndCommand = "Command";

        private readonly string eventName = "MessageJson";

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

        public override bool CheckEvent(string loggerName, string eventName) =>
            CheckLoggerName(loggerName)
            && CheckEventName(eventName);

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

            arguments.TryGetValue("database", out string database);
            arguments.TryGetValue("server", out string server);

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

            arguments.TryGetValue("parameters", out string parameters);
            arguments.TryGetValue("commandText", out string commandText);

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

            arguments.TryGetValue("elapsed", out string elapsed);
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
