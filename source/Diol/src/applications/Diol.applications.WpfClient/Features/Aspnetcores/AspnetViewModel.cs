using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.Features.Aspnetcores
{
    public class AspnetViewModel : BindableBase
    {
        private string _key;
        public string Key
        {
            get => this._key;
            set => SetProperty(ref this._key, value);
        }

        private string _method;
        public string Method
        {
            get => this._method;
            set => SetProperty(ref this._method, value);
        }

        private string _uri;
        public string Uri
        {
            get => this._uri;
            set => SetProperty(ref this._uri, value);
        }

        private int? _responseStatusCode;
        public int? ResponseStatusCode
        {
            get => this._responseStatusCode;
            set => SetProperty(ref this._responseStatusCode, value);
        }
    }
}
