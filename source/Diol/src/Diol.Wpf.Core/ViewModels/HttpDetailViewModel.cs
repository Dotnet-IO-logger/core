﻿using Diol.Share.Features.Httpclients;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Diol.Wpf.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// ViewModel for the HTTP detail view.
    /// </summary>
    public class HttpDetailViewModel : BindableBase
    {
        private HttpService service;
        private IEventAggregator eventAggregator;

        private RequestPipelineStartDto _request;
        /// <summary>
        /// Gets or sets the request object.
        /// </summary>
        public RequestPipelineStartDto Request
        {
            get => this._request;
            set => SetProperty(ref this._request, value);
        }

        /// <summary>
        /// Gets the collection of request query parameters.
        /// </summary>
        public ObservableCollection<KeyValuePair<string, string>> RequestQueryParameters { get; private set; } =
            new ObservableCollection<KeyValuePair<string, string>>();

        /// <summary>
        /// Gets the collection of request headers.
        /// </summary>
        public ObservableCollection<KeyValuePair<string, string>> RequestHeaders { get; private set; } =
            new ObservableCollection<KeyValuePair<string, string>>();

        private RequestPipelineEndDto _response;
        /// <summary>
        /// Gets or sets the response object.
        /// </summary>
        public RequestPipelineEndDto Response
        {
            get => this._response;
            set => SetProperty(ref this._response, value);
        }

        /// <summary>
        /// Gets the collection of response headers.
        /// </summary>
        public ObservableCollection<KeyValuePair<string, string>> ResponseHeaders { get; set; } =
            new ObservableCollection<KeyValuePair<string, string>>();

        public ICommand CopyCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDetailViewModel"/> class.
        /// </summary>
        /// <param name="service">The HTTP service.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public HttpDetailViewModel(
            HttpService service,
            IEventAggregator eventAggregator)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<HttpItemSelectedEvent>()
                .Subscribe(HandleHttpItemSelectedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Subscribe(HandleClearDataEvent, ThreadOption.UIThread);

            this.CopyCommand = new RelayCommand(CopySelectedData);
        }

        private DelegateCommand _closeCommand = null;
        /// <summary>
        /// Gets the command to close the HTTP detail view.
        /// </summary>
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(CloseExecute));

        private void CloseExecute()
        {
            this.eventAggregator
                .GetEvent<HttpItemSelectedEvent>()
                .Publish(string.Empty);
        }

        private void HandleClearDataEvent(string obj)
        {
            this.Request = null;
            this.RequestHeaders.Clear();
            this.Response = null;
            this.ResponseHeaders.Clear();
        }

        private void HandleHttpItemSelectedEvent(string obj)
        {
            this.HandleClearDataEvent(obj);

            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            this.Request = item.Request;

            if (item.Request.QueryParameters != null) 
            {
                foreach (var queryParameter in item.Request.QueryParameters)
                {
                    this.RequestQueryParameters.Add(new KeyValuePair<string, string>(queryParameter.Key, queryParameter.Value));
                }
            }

            if (item.RequestMetadata != null && item.RequestMetadata.Headers != null)
            {
                foreach (var header in item.RequestMetadata.Headers)
                {
                    this.RequestHeaders.Add(new KeyValuePair<string, string>(header.Key, header.Value));
                }
            }

            this.Response = item.Response;

            if (item.ResponseMetadata != null && item.ResponseMetadata.Headers != null)
            {
                foreach (var header in item.ResponseMetadata?.Headers)
                {
                    this.ResponseHeaders.Add(new KeyValuePair<string, string>(header.Key, header.Value));
                }
            }
        }

        private void CopySelectedData(object parameter)
        {
            if (parameter is DataGrid dataGrid && dataGrid.SelectedItems != null)
            {
                var sb = new StringBuilder();
                foreach (var item in dataGrid.SelectedItems)
                {
                    sb.AppendLine(item.ToString());
                }
                Clipboard.SetText(sb.ToString());
            }
        }
    }
}
