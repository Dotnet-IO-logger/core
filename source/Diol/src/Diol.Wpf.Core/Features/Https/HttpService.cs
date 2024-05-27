using Diol.Share.Features;
using Diol.Share.Features.Httpclients;
using Diol.Wpf.Core.Services;
using Prism.Events;

namespace Diol.Wpf.Core.Features.Https
{
    /// <summary>
    /// Represents an HTTP service for handling HTTP requests and responses.
    /// </summary>
    public class HttpService
    {
        private readonly IStore<HttpModel> store;
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class.
        /// </summary>
        /// <param name="store">The store for managing HTTP models.</param>
        /// <param name="eventAggregator">The event aggregator for publishing HTTP events.</param>
        public HttpService(
            IStore<HttpModel> store,
            IEventAggregator eventAggregator)
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets the HTTP model associated with the specified key, or the default value if not found.
        /// </summary>
        /// <param name="key">The key of the HTTP model.</param>
        /// <returns>The HTTP model associated with the specified key, or the default value if not found.</returns>
        public HttpModel GetItemOrDefault(string key)
        {
            return this.store.GetItemOrDefault(key);
        }

        /// <summary>
        /// Updates the HTTP service with the specified request pipeline start DTO.
        /// </summary>
        /// <param name="dto">The request pipeline start DTO.</param>
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

            this.eventAggregator
                .GetEvent<HttpRequestStartedEvent>()
                .Publish(dto.CorrelationId);
        }

        /// <summary>
        /// Updates the HTTP service with the specified request pipeline request header DTO.
        /// </summary>
        /// <param name="dto">The request pipeline request header DTO.</param>
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

        /// <summary>
        /// Updates the HTTP service with the specified request pipeline end DTO.
        /// </summary>
        /// <param name="dto">The request pipeline end DTO.</param>
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

        /// <summary>
        /// Updates the HTTP service with the specified request pipeline response header DTO.
        /// </summary>
        /// <param name="dto">The request pipeline response header DTO.</param>
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

        /// <summary>
        /// Updates the HTTP service with the specified base DTO.
        /// </summary>
        /// <param name="httpDto">The base DTO.</param>
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
            else if (httpDto is RequestPipelineResponseHeaderDto requestEndHeaderDto)
            {
                Update(requestEndHeaderDto);
            }
        }
    }
}
