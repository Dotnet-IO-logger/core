namespace Diol.Share.Services
{
    /// <summary>
    /// Represents an interface for subscribing to application state changes.
    /// </summary>
    public interface IApplicationStateService
    {
        /// <summary>
        /// Subscribes to application state changes.
        /// </summary>
        void Subscribe();
    }

    public class LocalApplicationStateService : IApplicationStateService
    {
        /// <inheritdoc/>
        public void Subscribe()
        {
        }
    }
}
