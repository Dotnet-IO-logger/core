using Diol.Share.Features;
using Diol.Share.Features.EntityFrameworks;
using Microsoft.Diagnostics.Tracing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Diol.Core.TraceEventProcessors
{
    public class EntityFrameworkProcessor : IProcessor
    {
        private readonly string loggerNameBegin = "Microsoft.EntityFrameworkCore.Database";
        private readonly string loggerNameEndConnection = "Connection";
        private readonly string loggerNameEndCommand = "Command";

        private readonly string eventName = "MessageJson";

        private EventPublisher eventObserver;

        public EntityFrameworkProcessor(EventPublisher eventObserver)
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
                && (name.EndsWith(this.loggerNameEndConnection)
                               || name.EndsWith(this.loggerNameEndCommand));
        }

        public void OnCompleted()
        {
            Debug.WriteLine($"{nameof(EntityFrameworkProcessor)} | {nameof(OnCompleted)}");
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine($"{nameof(EntityFrameworkProcessor)} | {nameof(OnError)}");
        }

        public void OnNext(TraceEvent value)
        {
            var eventId = Convert.ToInt32(value.PayloadByName("EventId"));
            var eventName = value.PayloadByName("EventName")?.ToString();

            Debug.WriteLine($"{value.ActivityID} | {eventName} | {eventId}");

            BaseDto be;

            if (eventId == 20000)
                be = ConnectionOpening(value);
            else if (eventId == 20100)
                be = CommandExecuting(value);
            else if (eventId == 20101)
                be = CommandExecuted(value);
            else
                be = null;

            // send notifications
            if (be != null)
            {
                be.ProcessName = value.ProcessName;
                be.ProcessId = value.ProcessID;

                this.eventObserver.AddEvent(be);
            }
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
                Elapsed = elapsedMs
            };
        }
    }
}
