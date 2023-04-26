using Diol.applications.WpfClient.Services;
using Diol.Share.Features;
using Diol.Share.Features.Httpclients;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.Features.Https
{
    public class HttpService
    {
        private readonly IStore<HttpModel> store;
        private readonly IEventAggregator eventAggregator;

        public HttpService(
            IStore<HttpModel> store,
            IEventAggregator eventAggregator)
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
        }

        public HttpModel GetItemOrDefault(string key) 
        {
            return this.store.GetItemOrDefault(key);
        }

        public void Update(RequestPipelineStartDto dto) 
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item != null) 
            {
                return;
            }

            item = new HttpModel() 
            {
                Key = dto.CorrelationId,
                Request = dto
            };

            this.store.Add(item, dto.CorrelationId);

            this.eventAggregator.GetEvent<HttpRequestStartedEvent>().Publish(dto.CorrelationId);
        }

        public void Update(RequestPipelineRequestHeaderDto dto)
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

        public void Update(RequestPipelineEndDto dto)
        {
            var item = this.store
                .GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.Response = dto;

            this.store.Update(item, dto.CorrelationId);

            this.eventAggregator.GetEvent<HttpRequestEndedEvent>().Publish(dto.CorrelationId);
        }

        public void Update(RequestPipelineResponseHeaderDto dto)
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

        public void Update(BaseDto httpDto) 
        {
            if (httpDto is RequestPipelineStartDto requestDto)
            {
                Update(requestDto);
            }
            else if (httpDto is RequestPipelineRequestHeaderDto requestHeaderDto)
            {
                Update(requestHeaderDto);
            }
            else if (httpDto is RequestPipelineEndDto requestEndDto)
            {
                Update(requestEndDto);
            }
            else if (httpDto is RequestPipelineRequestHeaderDto requestEndHeaderDto) 
            {
                Update(requestEndHeaderDto);
            }
        }
    }
}
