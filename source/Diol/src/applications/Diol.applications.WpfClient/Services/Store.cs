using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.Services
{
    public interface IStore<TEntity>
    {
        string Add(TEntity entity, string key);

        string Update(TEntity entity, string key);

        TEntity GetItemOrDefault(string id);

        void Clear();
    }

    public class Store<TEntity> : IStore<TEntity>
    {
        private Dictionary<string, TEntity> store = new Dictionary<string, TEntity>();
        
        public string Add(TEntity entity, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString();
            }
            this.store.Add(key, entity);
            return key;
        }
        
        public TEntity GetItemOrDefault(string id)
        {
            if (!string.IsNullOrEmpty(id) && this.store.ContainsKey(id))
            {
                return this.store[id];
            }
            return default;
        }

        public string Update(TEntity entity, string key)
        {
            if (this.store.ContainsKey(key))
            {
                this.store[key] = entity;
            }
            return key;
        }

        public void Clear()
        {
            this.store.Clear();
        }
    }
}
