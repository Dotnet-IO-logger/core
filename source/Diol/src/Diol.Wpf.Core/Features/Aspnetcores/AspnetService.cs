using Diol.Share.Features;
using Diol.Share.Features.Aspnetcores;
using Diol.Wpf.Core.Services;
using Prism.Events;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    /// <summary>
    /// Service class for handling ASP.NET Core related operations.
    /// </summary>
    public class AspnetService
    {
        private readonly IStore<AspnetcoreModel> store;
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspnetService"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public AspnetService(
            IStore<AspnetcoreModel> store,
            IEventAggregator eventAggregator)
        {
            this.store = store;
            this.eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets the item with the specified key from the store, or returns the default value if not found.
        /// </summary>
        /// <param name="key">The key of the item.</param>
        /// <returns>The item with the specified key, or the default value if not found.</returns>
        public AspnetcoreModel GetItemOrDefault(string key)
        {
            return this.store.GetItemOrDefault(key);
        }

        /// <summary>
        /// Updates the ASP.NET Core service with the specified request log data.
        /// </summary>
        /// <param name="dto">The request log data.</param>
        public void Update(RequestLogDto dto)
        {
            var item = this.store.GetItemOrDefault(dto.CorrelationId);

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

        /// <summary>
        /// Updates the ASP.NET Core service with the specified request body data.
        /// </summary>
        /// <param name="dto">The request body data.</param>
        public void Update(RequestBodyDto dto)
        {
            var item = this.store.GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.RequestMetadata = dto;

            this.store.Update(item, dto.CorrelationId);
        }

        /// <summary>
        /// Updates the ASP.NET Core service with the specified response log data.
        /// </summary>
        /// <param name="dto">The response log data.</param>
        public void Update(ResponseLogDto dto)
        {
            var item = this.store.GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.Response = dto;

            this.store.Update(item, dto.CorrelationId);

            this.eventAggregator
                .GetEvent<AspnetRequestEndedEvent>()
                .Publish(dto.CorrelationId);
        }

        /// <summary>
        /// Updates the ASP.NET Core service with the specified response body data.
        /// </summary>
        /// <param name="dto">The response body data.</param>
        public void Update(ResponseBodyDto dto)
        {
            var item = this.store.GetItemOrDefault(dto.CorrelationId);

            if (item == null)
            {
                return;
            }

            item.ResponseMetadata = dto;

            this.store.Update(item, dto.CorrelationId);
        }

        /// <summary>
        /// Updates the ASP.NET Core service with the specified ASP.NET DTO.
        /// </summary>
        /// <param name="aspnetDto">The ASP.NET DTO.</param>
        public void Update(BaseDto aspnetDto)
        {
            if (aspnetDto is RequestLogDto requestLogDto)
                Update(requestLogDto);
            else if (aspnetDto is RequestBodyDto bodyDto)
                Update(bodyDto);
            else if (aspnetDto is ResponseLogDto responseLogDto)
                Update(responseLogDto);
            else if (aspnetDto is ResponseBodyDto responseBodyDto)
                Update(responseBodyDto);
        }
    }
}
