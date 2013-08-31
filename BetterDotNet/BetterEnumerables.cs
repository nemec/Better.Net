using System.Collections.Generic;
using System.Linq;

namespace BetterDotNet
{
    /// <summary>
    /// Adds extension methods for enumerables.
    /// </summary>
    public static class BetterEnumerablesExtensions
    {
        /// <summary>
        /// Split an enumerable into N equal enumerables (to prevent issues
        /// with "multiple enumeration of IEnumerable"). After being split,
        /// the original enumerable should not be used anywhere else,
        /// otherwise the tee'd enumerables may lose data.
        /// 
        /// Note: values consumed by the sub enumerables are cached in
        /// memory until ALL sub enumerables have consumed that value,
        /// which may require a significant amount of storage if one
        /// enumerable is evaluated many times more than the others. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="numSubEnumerables">Number of sub enumerables to create.</param>
        /// <returns></returns>
        public static IEnumerable<T>[] Tee<T>(this IEnumerable<T> source, int numSubEnumerables)
        {
            var cache = new EnumerableCache<T>(source.GetEnumerator());
            var arr = new IEnumerable<T>[numSubEnumerables];
            for (var i = 0; i < numSubEnumerables; i++)
            {
                arr[i] = SubEnumerable(cache.Register());
            }
            return arr;
        }

        /// <summary>
        /// Split an enumerable into N equal enumerables (to prevent issues
        /// with "multiple enumeration of IEnumerable"). After being split,
        /// the original enumerable should not be used anywhere else,
        /// otherwise the tee'd enumerables may lose data.
        /// 
        /// It is safe to consume each enumerable in a separate thread.
        /// 
        /// Note: values consumed by the sub enumerables are cached in
        /// memory until ALL sub enumerables have consumed that value,
        /// which may require a significant amount of storage if one
        /// enumerable is evaluated many times more than the others. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="numSubEnumerables">Number of sub enumerables to create.</param>
        /// <returns></returns>
        public static IEnumerable<T>[] ConcurrentTee<T>(this IEnumerable<T> source, int numSubEnumerables)
        {
            var cache = new EnumerableCache<T>(source.GetEnumerator());
            var arr = new IEnumerable<T>[numSubEnumerables];
            for (var i = 0; i < numSubEnumerables; i++)
            {
                arr[i] = ConcurrentSubEnumerable(cache.Register());
            }
            return arr;
        }

        private static IEnumerable<T> SubEnumerable<T>(EnumerableToken<T> tok)
        {
            T item;
            while (tok.TryConsume(out item))
            {
                yield return item;
            }
        }

        private static IEnumerable<T> ConcurrentSubEnumerable<T>(EnumerableToken<T> tok)
        {
            T item;
            while (tok.TryConsumeConcurrent(out item))
            {
                yield return item;
            }
        } 

        private class EnumerableCache<T>
        {
            private readonly List<T> _cache;

            private readonly IEnumerator<T> _iter;

            private readonly object _lock = new object();

            private Dictionary<EnumerableToken<T>, int> ConsumedIndex { get; set; } 

            public EnumerableCache(IEnumerator<T> iter)
            {
                _cache = new List<T>();
                _iter = iter;
                ConsumedIndex = new Dictionary<EnumerableToken<T>, int>();
            }

            public EnumerableToken<T> Register()
            {
                var tok = new EnumerableToken<T>(this);
                ConsumedIndex[tok] = 0;
                return tok;
            }

            public bool TryConsumeConcurrent(EnumerableToken<T> tok, out T item)
            {
                lock (_lock)
                {
                    return TryConsume(tok, out item);
                }
            }

            public bool TryConsume(EnumerableToken<T> tok, out T item)
            {
                item = default(T);
                if (ConsumedIndex.ContainsKey(tok))
                {
                    var offset = ConsumedIndex[tok];
                    if (ConsumedIndex.Count > 1 &&
                        offset == 0 && 
                        ConsumedIndex.Values.Count(o => o == 0) == 1) // This is the only one at index 0
                    {
                        foreach (var key in ConsumedIndex.Keys.ToList())
                        {
                            if (!ReferenceEquals(key, tok))
                            {
                                ConsumedIndex[key]--; // Decrement the others
                            }
                        }
                        item = _cache[0];
                        _cache.RemoveAt(0);
                        return true;
                    }

                    offset++;
                    ConsumedIndex[tok] = offset;

                    if (offset > _cache.Count)
                    {
                        if (!_iter.MoveNext())
                        {
                            ConsumedIndex[tok]--;  // We can't move any further. Turn back!
                            return false;
                        }
                        _cache.Add(_iter.Current);
                    }

                    item = _cache[offset - 1];
                    return true;
                }

                return false;
            }
        }

        private class EnumerableToken<T>
        {
            private EnumerableCache<T> Cache { get; set; }

            public EnumerableToken(EnumerableCache<T> cache)
            {
                Cache = cache;
            }

            public bool TryConsume(out T item)
            {
                return Cache.TryConsume(this, out item);
            }

            public bool TryConsumeConcurrent(out T item)
            {
                return Cache.TryConsumeConcurrent(this, out item);
            }
        }
    }
}
