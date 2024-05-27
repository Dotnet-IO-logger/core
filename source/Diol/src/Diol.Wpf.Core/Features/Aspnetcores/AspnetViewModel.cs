using Prism.Mvvm;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    /// <summary>
    /// Represents the view model for ASP.NET core features.
    /// </summary>
    public class AspnetViewModel : BindableBase
    {
        private string _key;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key
        {
            get => this._key;
            set => SetProperty(ref this._key, value);
        }

        private string _method;

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        public string Method
        {
            get => this._method;
            set => SetProperty(ref this._method, value);
        }

        private string _uri;

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        public string Uri
        {
            get => this._uri;
            set => SetProperty(ref this._uri, value);
        }

        private int? _responseStatusCode;

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        public int? ResponseStatusCode
        {
            get => this._responseStatusCode;
            set => SetProperty(ref this._responseStatusCode, value);
        }
    }
}
