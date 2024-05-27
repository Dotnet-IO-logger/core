using System;
using System.Collections.Generic;

namespace Diol.Wpf.Core.Services
{
    /// <summary>
    /// Represents a generic store interface for storing entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IStore<TEntity>
    {
        /// <summary>
        /// Adds an entity to the store with the specified key.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="key">The key associated with the entity.</param>
        /// <returns>The key of the added entity.</returns>
        string Add(TEntity entity, string key);

        /// <summary>
        /// Updates an entity in the store with the specified key.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="key">The key associated with the entity.</param>
        /// <returns>The key of the updated entity.</returns>
        string Update(TEntity entity, string key);

        /// <summary>
        /// Gets an item from the store with the specified id.
        /// </summary>
        /// <param name="id">The id of the item to get.</param>
        /// <returns>The item with the specified id, or the default value of TEntity if not found.</returns>
        TEntity GetItemOrDefault(string id);

        /// <summary>
        /// Clears the store.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Represents a local store implementation using a dictionary.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class LocalStore<TEntity> : IStore<TEntity>
    {
        private Dictionary<string, TEntity> store = new Dictionary<string, TEntity>();

        /// <inheritdoc/>
        public string Add(TEntity entity, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString();
            }
            this.store.Add(key, entity);
            return key;
        }

        /// <inheritdoc/>
        public TEntity GetItemOrDefault(string id)
        {
            if (!string.IsNullOrEmpty(id) && this.store.ContainsKey(id))
            {
                return this.store[id];
            }
            return default;
        }

        /// <inheritdoc/>
        public string Update(TEntity entity, string key)
        {
            if (this.store.ContainsKey(key))
            {
                this.store[key] = entity;
            }
            return key;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.store.Clear();
        }
    }
}
