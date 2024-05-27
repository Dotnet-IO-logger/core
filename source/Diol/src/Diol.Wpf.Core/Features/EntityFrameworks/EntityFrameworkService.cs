using Diol.Share.Features;
using Diol.Share.Features.EntityFrameworks;
using Diol.Wpf.Core.Services;
using Prism.Events;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    /// <summary>
    /// Service class for interacting with Entity Framework.
    /// </summary>
    public class EntityFrameworkService
    {
        private readonly IStore<EntityFrameworkModel> store;
        private readonly IEventAggregator eventAggregator;

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
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.CommandExecuting = dto;

            this.store.Update(item, dto.CorrelationId);
        }

        /// <summary>
        /// Updates the Entity Framework service with the specified command executed DTO.
        /// </summary>
        /// <param name="dto">The command executed DTO.</param>
        public void Update(CommandExecutedDto dto)
        {
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
