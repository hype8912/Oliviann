#if !NETFRAMEWORK
namespace Oliviann.Extensions.Caching.Memory
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.Extensions.Caching.Memory;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="MemoryCache"/> objects.
    /// </summary>
    public static class MemoryCacheExtensions
    {
        /// <summary>
        /// Gets all the keys for the specified cache.
        /// </summary>
        /// <param name="cache">The cache instance.</param>
        /// <returns>A collection of available keys.</returns>
        public static IReadOnlyCollection<string> GetKeys(this MemoryCache cache)
        {
            var keys = new List<string>();
            if (cache == null)
            {
                return keys;
            }

            PropertyInfo property = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            if (!(property?.GetValue(cache) is ICollection entries))
            {
                return keys;
            }

            Type entryType = null;
            foreach (object entry in entries)
            {
                if (entryType == null)
                {
                    // Stores the entry type so we don't keep double calling to
                    // reflection for the type since the type of the collection
                    // doesn't change.
                    entryType = entry.GetType();
                }

                PropertyInfo keyProperty = entryType.GetProperty("Key");
                if (keyProperty == null)
                {
                    continue;
                }

                string key = keyProperty.GetValue(entry).ToString();
                keys.Add(key);
            }

            return keys;
        }
    }
}

#endif