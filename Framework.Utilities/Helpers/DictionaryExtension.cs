using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Framework.Utilities.Helpers
{
    /// <summary>
    /// An Extension of a Dictionary.
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        ///   Gets the value of by key in a dictionary and returns default value if the Key is not found.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key to be found.</param>
        /// <param name="defaultValueFactory">The factory to create default value.</param>
        /// <returns>The value associated with the key or default value if not found.</returns>
        public static TValue ValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> defaultValueFactory = null)
        {
            TValue value;
            if (defaultValueFactory == null)
            {
                defaultValueFactory = () => default(TValue);
            }

            if (key == null || dictionary == null)
            {
                return defaultValueFactory();
            }

            if (!dictionary.TryGetValue(key, out value))
            {
                value = defaultValueFactory();
            }

            return value;
        }

        //// The "valueFactory" provides a lazy evaluated value and thus avoid possible expensive overhead of
        //// evaluating a value in case there is already existing element.
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> valueFactory)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            value = valueFactory();
            dictionary[key] = value;

            return value;
        }

        public static bool TryVisit<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Action<TValue> visitor)
        {
            return TryVisit(
                dictionary,
                key,
                entry =>
                    {
                        visitor(entry);
                        return true;
                    });
        }

        public static TResult TryVisit<TKey, TValue, TResult>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue, TResult> visitor)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return visitor(value);
            }

            return default(TResult);
        }

        /// <summary>
        /// Removes an entry of a specified key from a concurrent dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <param name="dictionary">The concurrent dictionary.</param>
        /// <param name="key">The key to find.</param>
        /// <returns>A value indicating if the specified key is existing or not.</returns>
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryRemove(key, out value);
        }
    }
}