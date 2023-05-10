using Diol.Share.Features;
using Diol.Share.Features.Aspnetcores;
using Diol.Wpf.Core.Services;
using Prism.Events;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    public class AspnetService
    {
        private readonly IStore<AspnetcoreModel> store;
        private readonly IEventAggregator eventAggregator;

        public AspnetService(
            IStore<AspnetcoreModel> store,
            IEventAggregator eventAggregator)
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
        }

        public AspnetcoreModel GetItemOrDefault(string key)
        {
            return this.store.GetItemOrDefault(key);
        }

        public void Update(RequestLogDto dto) 
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item != null)
            {
                return;
            }

            item = new AspnetcoreModel()
            {
                Key = dto.CorrelationId,
                Request = dto
            };

            this.store.Add(item, dto.CorrelationId);

            this.eventAggregator
                .GetEvent<AspnetRequestStartedEvent>()
                .Publish(dto.CorrelationId);
        }

        public void Update(RequestBodyDto dto)
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.RequestMetadata = dto;

            this.store.Update(item, dto.CorrelationId);
        }

        public void Update(ResponseLogDto dto)
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.Response = dto;

            this.store.Update(item, dto.CorrelationId);

            this.eventAggregator
                .GetEvent<AspnetRequestEndedEvent>().
                Publish(dto.CorrelationId);
        }

        public void Update(ResponseBodyDto dto)
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.ResponseMetadata = dto;

            this.store.Update(item, dto.CorrelationId);
        }

        public void Update(BaseDto aspnetDto) 
        {
            if (aspnetDto is RequestLogDto requestLogDto) 
                Update(requestLogDto);
            else if (aspnetDto is RequestBodyDto bodyDto)
                Update(bodyDto);
            else if(aspnetDto is ResponseLogDto responseLogDto)
                Update(responseLogDto);
            else if(aspnetDto is ResponseBodyDto responseBodyDto)
                Update(responseBodyDto);
        }
    }
}
