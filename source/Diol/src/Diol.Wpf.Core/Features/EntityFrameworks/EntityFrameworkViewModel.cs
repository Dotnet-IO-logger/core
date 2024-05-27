using Prism.Mvvm;
using System;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    /// <summary>
    /// Represents a view model for Entity Framework.
    /// </summary>
    public class EntityFrameworkViewModel : BindableBase
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

        private string _server;

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        public string Server
        {
            get => this._server;
            set => SetProperty(ref this._server, value);
        }

        private string _database;

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database
        {
            get => this._database;
            set => SetProperty(ref this._database, value);
        }

        private TimeSpan? _durationInMiliSeconds;

        /// <summary>
        /// Gets or sets the duration in milliseconds.
        /// </summary>
        public TimeSpan? DurationInMiliSeconds
        {
            get => this._durationInMiliSeconds;
            set => SetProperty(ref this._durationInMiliSeconds, value);
        }
    }
}
