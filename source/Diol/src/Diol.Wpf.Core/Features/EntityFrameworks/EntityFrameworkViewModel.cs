using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    public class EntityFrameworkViewModel : BindableBase
    {
        private string _key;
        public string Key
        {
            get => this._key;
            set => SetProperty(ref this._key, value);
        }

        private string _server;
        public string Server
        {
            get => this._server;
            set => SetProperty(ref this._server, value);
        }

        private string _database;
        public string Database
        {
            get => this._database;
            set => SetProperty(ref this._database, value);
        }

        private TimeSpan? _durationInMiliSeconds;
        public TimeSpan? DurationInMiliSeconds
        {
            get => this._durationInMiliSeconds;
            set => SetProperty(ref this._durationInMiliSeconds, value);
        }
    }
}
