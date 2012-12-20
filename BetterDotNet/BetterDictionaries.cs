using System.Collections.Generic;

// ReSharper disable PartialTypeWithSinglePart
namespace BetterDotNet
{
    /// <summary>
    /// Extension class dealing with dictionaries.
    /// </summary>
    public static partial class BetterDictionaries
    {
        /// <summary>
        ///     Attempt to retrieve a value from the collection and,
        ///     if the key does not exist, return a default value.
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dictionary">Retrieve value from this</param>
        /// <param name="key">Key to retrieve</param>
        /// <param name="defaultValue">If key is not present, return this value</param>
        /// <returns>
        ///     The value at the given key, if present, or the default value.
        /// </returns>
        public static TValue Get<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ?
                value :
                defaultValue;
        }
    }
}
