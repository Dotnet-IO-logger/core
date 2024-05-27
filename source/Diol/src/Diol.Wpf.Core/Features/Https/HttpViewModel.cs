using Prism.Mvvm;
using System;

namespace Diol.Wpf.Core.Features.Https
{
    /// <summary>
    /// Represents a view model for making HTTP requests.
    /// </summary>
    public class HttpViewModel : BindableBase
    {
        private string _key;

        /// <summary>
        /// Gets or sets the key for the HTTP request.
        /// </summary>
        public string Key
        {
            get => this._key;
            set => SetProperty(ref this._key, value);
        }

        private string _method;

        /// <summary>
        /// Gets or sets the HTTP method for the request.
        /// </summary>
        public string Method
        {
            get => this._method;
            set => SetProperty(ref this._method, value);
        }

        private string _uri;

        /// <summary>
        /// Gets or sets the URI for the HTTP request.
        /// </summary>
        public string Uri
        {
            get => this._uri;
            set => SetProperty(ref this._uri, value);
        }

        private int? _responseStatusCode;

        /// <summary>
        /// Gets or sets the response status code of the HTTP request.
        /// </summary>
        public int? ResponseStatusCode
        {
            get => this._responseStatusCode;
            set => SetProperty(ref this._responseStatusCode, value);
        }

        private TimeSpan? _durationInMiliSeconds;

        /// <summary>
        /// Gets or sets the duration of the HTTP request in milliseconds.
        /// </summary>
        public TimeSpan? DurationInMiliSeconds
        {
            get => this._durationInMiliSeconds;
            set => SetProperty(ref this._durationInMiliSeconds, value);
        }
    }
}
