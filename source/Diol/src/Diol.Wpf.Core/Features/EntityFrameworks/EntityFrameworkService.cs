using Diol.Share.Features;
using Diol.Share.Features.EntityFrameworks;
using Diol.Wpf.Core.Services;
using Prism.Events;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    public class EntityFrameworkService
    {
        private readonly IStore<EntityFrameworkModel> store;
        private readonly IEventAggregator eventAggregator;

        public EntityFrameworkService(
            LocalStore<EntityFrameworkModel> store,
            IEventAggregator eventAggregator)
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
        }

        public EntityFrameworkModel GetItemOrDefault(string key)
        {
            return this.store.GetItemOrDefault(key);
        }

        public void Update(ConnectionOpeningDto dto) 
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if(item != null)
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

        public void Update(BaseDto entityFrameworkDto) 
        {
            if(entityFrameworkDto is ConnectionOpeningDto connectionOpeningDto)
                this.Update(connectionOpeningDto);
            else if(entityFrameworkDto is CommandExecutingDto commandExecutingDto)
                this.Update(commandExecutingDto);
            else if(entityFrameworkDto is CommandExecutedDto commandExecutedDto)
                this.Update(commandExecutedDto);
        }
    }
}
