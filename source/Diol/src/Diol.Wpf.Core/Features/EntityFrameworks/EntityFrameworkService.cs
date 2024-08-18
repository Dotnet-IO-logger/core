using Diol.Share.Features;
using Diol.Share.Features.EntityFrameworks;
using Diol.Wpf.Core.Services;
using Prism.Events;
using System.Collections.Generic;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    /// <summary>
    /// Service class for interacting with Entity Framework.
    /// </summary>
    public class EntityFrameworkService
    {
        private readonly IStore<EntityFrameworkModel> store;
        private readonly IEventAggregator eventAggregator;

        private readonly Dictionary<string, int> multipleQueries = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkService"/> class.
        /// </summary>
        /// <param name="store">The store for Entity Framework models.</param>
        /// <param name="eventAggregator">The event aggregator for publishing events.</param>
        public EntityFrameworkService(
            LocalStore<EntityFrameworkModel> store,
            IEventAggregator eventAggregator)
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets the Entity Framework model item with the specified key, or the default value if not found.
        /// </summary>
        /// <param name="key">The key of the item to retrieve.</param>
        /// <returns>The Entity Framework model item with the specified key, or the default value if not found.</returns>
        public EntityFrameworkModel GetItemOrDefault(string key)
        {
            if (this.multipleQueries.Count >= 128) 
            {
                this.multipleQueries.Clear();
            }

            return this.store.GetItemOrDefault(key);
        }

        /// <summary>
        /// Updates the Entity Framework service with the specified connection opening DTO.
        /// </summary>
        /// <param name="dto">The connection opening DTO.</param>
        public void Update(ConnectionOpeningDto dto)
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item != null)
            {
                return;
            }

            item = new EntityFrameworkModel()
            {
                Key = dto.CorrelationId,
                ConnectionOpening = dto
            };

            this.store.Add(item, dto.CorrelationId);

            this.eventAggregator
                .GetEvent<EntityFrameworkConnectionStartedEvent>()
                .Publish(dto.CorrelationId);
        }

        /// <summary>
        /// Updates the Entity Framework service with the specified command executing DTO.
        /// </summary>
        /// <param name="dto">The command executing DTO.</param>
        public void Update(CommandExecutingDto dto)
        {
            if(!this.multipleQueries.ContainsKey(dto.CorrelationId))
            {
                this.multipleQueries.Add(dto.CorrelationId, 0);
            }
            else
            {
                this.multipleQueries[dto.CorrelationId]++;
                var relationId = this.multipleQueries[dto.CorrelationId];

                var correlationId = $"{dto.CorrelationId}_{relationId}";

                var prevItem = this.store
                    .GetItemOrDefault(dto.CorrelationId);

                if (prevItem == null) 
                {
                    return;
                }

                var processId = prevItem.ConnectionOpening.ProcessId;
                var processName = prevItem.ConnectionOpening.ProcessName;
                var server = prevItem.ConnectionOpening.Server;
                var databaseName = prevItem.ConnectionOpening.Database;

                var createItem = new ConnectionOpeningDto() 
                {
                    CorrelationId = correlationId,
                    ProcessId = processId,
                    ProcessName = processName,
                    Server = server,
                    Database = databaseName,
                };

                this.Update(createItem);

                dto.CorrelationId = correlationId;
            }

            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.CommandExecuting = dto;

            this.eventAggregator
                .GetEvent<EntityFrameworkQueryExecutingEvent>()
                .Publish(dto.CorrelationId);

            this.store.Update(item, dto.CorrelationId);
        }

        /// <summary>
        /// Updates the Entity Framework service with the specified command executed DTO.
        /// </summary>
        /// <param name="dto">The command executed DTO.</param>
        public void Update(CommandExecutedDto dto)
        {
            if(this.multipleQueries.ContainsKey(dto.CorrelationId))
            {
                var relationId = this.multipleQueries[dto.CorrelationId];
                var correlationId = relationId > 0 ? $"{dto.CorrelationId}_{relationId}" : dto.CorrelationId;
                dto.CorrelationId = correlationId;
            }

            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.CommandExecuted = dto;

            this.store.Update(item, dto.CorrelationId);

            this.eventAggregator
                .GetEvent<EntityFrameworkConnectionEndedEvent>()
                .Publish(dto.CorrelationId);
        }

        /// <summary>
        /// Updates the Entity Framework service with the specified Entity Framework DTO.
        /// </summary>
        /// <param name="entityFrameworkDto">The Entity Framework DTO.</param>
        public void Update(BaseDto entityFrameworkDto)
        {
            if (entityFrameworkDto is ConnectionOpeningDto connectionOpeningDto)
                this.Update(connectionOpeningDto);
            else if (entityFrameworkDto is CommandExecutingDto commandExecutingDto)
                this.Update(commandExecutingDto);
            else if (entityFrameworkDto is CommandExecutedDto commandExecutedDto)
                this.Update(commandExecutedDto);
        }
    }
}
