using System.Collections.Generic;

namespace Diol.Core.Utils
{
    public static class Utils
    {
        /// <summary>
        /// Tries to get the value associated with the specified key from the dictionary and removes the key-value pair if found.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to perform the operation on.</param>
        /// <param name="key">The key to look for in the dictionary.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key if found; otherwise, the default value for the type of the value parameter.</param>
        public static void TryGetValueAndRemove<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            TKey key,
            out TValue value)
        {
            if (dictionary.TryGetValue(key, out value))
                dictionary.Remove(key);
        }
    }
}
