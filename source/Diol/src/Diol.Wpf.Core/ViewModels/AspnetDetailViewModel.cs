using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Shared;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// ViewModel for the AspnetDetail view.
    /// </summary>
    public class AspnetDetailViewModel : BindableBase
    {
        private AspnetService service;
        private IEventAggregator eventAggregator;

        private string _uri;
        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        public string Uri
        {
            get => this._uri;
            set => SetProperty(ref this._uri, value);
        }

        private string _method;
        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public string Method
        {
            get => this._method;
            set => SetProperty(ref this._method, value);
        }

        private string _protocol;
        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        public string Protocol
        {
            get => this._protocol;
            set => SetProperty(ref this._protocol, value);
        }

        private int? _statusCode;
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public int? StatusCode
        {
            get => this._statusCode;
            set => SetProperty(ref this._statusCode, value);
        }

        private string _requestBodyAsString;
        /// <summary>
        /// Gets or sets the request body as a string.
        /// </summary>
        public string RequestBodyAsString
        {
            get => this._requestBodyAsString;
            set => SetProperty(ref this._requestBodyAsString, value);
        }

        private string _responseBodyAsString;
        /// <summary>
        /// Gets or sets the response body as a string.
        /// </summary>
        public string ResponseBodyAsString
        {
            get => this._responseBodyAsString;
            set => SetProperty(ref this._responseBodyAsString, value);
        }

        /// <summary>
        /// Gets the collection of request headers.
        /// </summary>
        public ObservableCollection<KeyValuePair<string, string>> RequestHeaders { get; private set; } =
            new ObservableCollection<KeyValuePair<string, string>>();

        /// <summary>
        /// Gets or sets the collection of response headers.
        /// </summary>
        public ObservableCollection<KeyValuePair<string, string>> ResponseHeaders { get; set; } =
            new ObservableCollection<KeyValuePair<string, string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AspnetDetailViewModel"/> class.
        /// </summary>
        /// <param name="service">The AspnetService instance.</param>
        /// <param name="eventAggregator">The IEventAggregator instance.</param>
        public AspnetDetailViewModel(
            AspnetService service,
            IEventAggregator eventAggregator)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<AspnetItemSelectedEvent>()
                .Subscribe(HandleAspnetItemSelectedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Subscribe(HandleClearDataEvent, ThreadOption.UIThread);
        }

        private DelegateCommand _closeCommand = null;
        /// <summary>
        /// Gets the command to close the view.
        /// </summary>
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(CloseExecute));

        private void CloseExecute()
        {
            this.eventAggregator
                .GetEvent<AspnetItemSelectedEvent>()
                .Publish(string.Empty);
        }

        private void HandleAspnetItemSelectedEvent(string obj)
        {
            this.HandleClearDataEvent(obj);

            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            this.Method = item.Request.Method;
            this.Uri = $"{item.Request.Scheme}://{item.Request.Host}{item.Request.Path}";
            this.Protocol = item.Request.Protocol;
            this.StatusCode = item?.Response?.StatusCode;

            if (item.Request.Metadata != null)
            {
                foreach (var header in item.Request.Metadata)
                {
                    this.RequestHeaders.Add(new KeyValuePair<string, string>(header.Key, header.Value));
                }
            }

            this.RequestBodyAsString = item?.RequestMetadata?.BodyAsString;

            if (item.Response != null && item.Response.Metadata != null)
            {
                foreach (var header in item.Response.Metadata)
                {
                    this.ResponseHeaders.Add(new KeyValuePair<string, string>(header.Key, header.Value));
                }
            }

            this.ResponseBodyAsString = item?.ResponseMetadata?.BodyAsString;
        }

        private void HandleClearDataEvent(string obj)
        {
            this.Uri = string.Empty;
            this.Method = string.Empty;
            this.StatusCode = int.MinValue;

            this.RequestHeaders.Clear();
            this.RequestBodyAsString = string.Empty;

            this.ResponseHeaders.Clear();
            this.ResponseBodyAsString = string.Empty;
        }
    }
}
