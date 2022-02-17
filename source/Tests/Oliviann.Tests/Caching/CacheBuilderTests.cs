namespace Oliviann.Tests.Caching
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Caching;
    using Oliviann.Caching.Providers;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class CacheBuilderTests
    {
        #region Add Provider Tests

        /// <summary>
        /// Verifies adding a provider doesn't throw and exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());

            ICache cache = builder;
            Assert.NotNull(cache);
        }

        /// <summary>
        /// Verifies adding a null provider throws an exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddProvider_NullProvider()
        {
            var builder = new CacheBuilder();
            ICacheProvider provider = null;

            Assert.Throws<ArgumentNullException>(() => builder.AddProvider(provider));
        }

        /// <summary>
        /// Verifies adding a provider with a whitespace name throws an
        /// exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddProvider_WhiteSpaceProviderName()
        {
            var builder = new CacheBuilder();
            ICacheProvider provider = new MemoryCacheProvider("            ");

            Assert.Throws<ArgumentException>(() => builder.AddProvider(provider));
        }

        /// <summary>
        /// Verifies updating a provider with the same name doesn't throw an
        /// exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddProvider_UpdateProvider()
        {
            var builder = new CacheBuilder();

            ICacheProvider provider1 = new MemoryCacheProvider("Taco");
            builder.AddProvider(provider1);

            ICacheProvider provider2 = new MemoryCacheProvider("Taco");
            builder.AddProvider(provider2);
        }

        #endregion Add Provider Tests

        #region Add Tests

        /// <summary>
        /// Verifies adding a null entry returns false.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddEntry_NullEntry()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            bool result = cache.Add(null);
            Assert.False(result);
        }

        /// <summary>
        /// Verifies adding an entry with a bad key returns false.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        public void CacheBuilderTest_AddEntry_BadKey(string key)
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            var entry = new CacheEntry(key);
            bool result = cache.Add(entry);
            Assert.False(result);
        }

        /// <summary>
        /// Verifies adding an entry with no providers returns false.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddEntry_NoProviders()
        {
            var builder = new CacheBuilder();
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            bool result = cache.Add(entry);
            Assert.False(result);
        }

        /// <summary>
        /// Verifies adding a null entry returns false.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddEntry_SingleProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Water"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            bool result = cache.Add(entry);
            Assert.True(result);
            Assert.True(cache.Contains(entry));
        }

        /// <summary>
        /// Verifies adding a null entry returns false.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_AddEntry_MultipleProviders()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Wood"));
            builder.AddProvider(new MemoryCacheProvider("Food"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            bool result = cache.Add(entry);
            Assert.True(result);

            Assert.True(cache.Contains(entry));
        }

        #endregion Add Tests

        #region Contains Tests

        /// <summary>
        /// Verifies adding a null entry returns false.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_ContainsEntry_NullEntry()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            bool result = cache.Contains(null);
            Assert.False(result);
        }

        /// <summary>
        /// Verifies an entry with a bad key returns false.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        public void CacheBuilderTest_ContainsEntry_BadKey(string key)
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            var entry = new CacheEntry(key);
            cache.Add(entry);
            bool result = cache.Contains(entry);
            Assert.False(result);
        }

        /// <summary>
        /// Verifies an a missing entry returns false.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_ContainsEntry_MissingEntry_SingleProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            cache.Add(entry);
            Assert.False(cache.Contains(new CacheEntry("Burger", "King")));
        }

        /// <summary>
        /// Verifies an a missing entry returns false.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_ContainsEntry_MissingEntry_MultipleProviders()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            builder.AddProvider(new MemoryCacheProvider("Contains"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            cache.Add(entry);
            Assert.False(cache.Contains(new CacheEntry("Burger", "King")));
        }

        #endregion Contains Tests

        #region Get Tests

        /// <summary>
        /// Verifies getting a null entry returns null.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_GetEntry_NullEntry()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            object result = cache.Get(null);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies getting an entry with a bad key returns null.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        public void CacheBuilderTest_GetEntry_BadKey(string key)
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            var entry = new CacheEntry(key);
            object result = cache.Get(entry);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies getting an entry with no providers returns null.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_GetEntry_NoProviders()
        {
            var builder = new CacheBuilder();
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            object result = cache.Get(entry);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies getting a missing entry with multiple providers returns
        /// null.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_GetEntry_MultipleProviders()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Wood1"));
            builder.AddProvider(new MemoryCacheProvider("Food1"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            Assert.True(cache.Add(entry));
            Assert.True(cache.Contains(entry));

            object result = cache.Get(new CacheEntry("Burger", "King"));
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies adding and getting an entry from a specific provider
        /// returns the correct result.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_GetEntry_SpecificProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Wood2"));
            builder.AddProvider(new MemoryCacheProvider("Food2"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            Assert.True(cache.Add(entry, "Wood2"));
            Assert.True(cache.Contains(entry, "Wood2"));

            object result1 = cache.Get(entry, "Food2");
            Assert.Null(result1);

            object result2 = cache.Get(entry, "Wood2");
            Assert.Equal("Bell", result2);
        }

        #endregion Get Tests

        #region GetKeys Tests

        /// <summary>
        /// Verifies getting keys with no providers returns an empty collection.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_GetKeys_NoProviders()
        {
            var builder = new CacheBuilder();
            ICache cache = builder;

            IReadOnlyCollection<string> result = cache.GetKeys();
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Verifies get keys with no provider returns all keys in all
        /// providers.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_GetKeys_MultipleProviders()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Wood3"));
            builder.AddProvider(new MemoryCacheProvider("Food3"));
            ICache cache = builder;

            var entry1 = new CacheEntry("Taco", "Bell");
            Assert.True(cache.Add(entry1, "Food3"));
            Assert.True(cache.Contains(entry1));

            var entry2 = new CacheEntry("Oak", "Strong");
            Assert.True(cache.Add(entry2, "Wood3"));
            Assert.True(cache.Contains(entry2));

            IReadOnlyCollection<string> result = cache.GetKeys();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains("Taco", result);
            Assert.Contains("Oak", result);
        }

        /// <summary>
        /// Verifies get keys with no provider returns all keys in all
        /// providers.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_GetKeys_SpecificProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Wood4"));
            builder.AddProvider(new MemoryCacheProvider("Food4"));
            ICache cache = builder;

            var entry1 = new CacheEntry("Taco", "Bell");
            Assert.True(cache.Add(entry1, "Food4"));
            Assert.True(cache.Contains(entry1));

            var entry2 = new CacheEntry("Oak", "Strong");
            Assert.True(cache.Add(entry2, "Wood4"));
            Assert.True(cache.Contains(entry2));

            IReadOnlyCollection<string> result = cache.GetKeys("Wood4");
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains("Oak", result);
            Assert.DoesNotContain(result, r => r == "Bell");
        }

        #endregion

        #region Set Tests

        /// <summary>
        /// Verifies setting a null entry doesn't throw and exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_SetEntry_NullEntry()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            cache.Set(null);
        }

        /// <summary>
        /// Verifies setting an entry with a bad key doesn't throw an exception.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        public void CacheBuilderTest_SetEntry_BadKey(string key)
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            var entry = new CacheEntry(key);
            cache.Set(entry);
        }

        /// <summary>
        /// Verifies setting an entry with no providers doesn't throw an
        /// exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_SetEntry_NoProviders()
        {
            var builder = new CacheBuilder();
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            cache.Set(entry);
        }

        /// <summary>
        /// Verifies setting a missing entry adds it to the cache.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_SetMissingEntry_SingleProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Set1"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            Assert.False(cache.Contains(entry));

            cache.Set(entry);
            Assert.True(cache.Contains(entry));
        }

        /// <summary>
        /// Verifies setting a value that already exists in the cache doesn't
        /// throw an exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_SetUpdatingEntryValue_SingleProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Set2"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            Assert.False(cache.Contains(entry));

            cache.Set(entry);
            Assert.True(cache.Contains(entry));

            entry.Value = "Cabana";
            cache.Set(entry);
            Assert.True(cache.Contains(entry));

            object result = cache.Get(entry);
            Assert.Equal("Cabana", (string)result);
        }

        /// <summary>
        /// Verifies setting a value that already exists in the cache doesn't
        /// throw an exception.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_SetUpdatingEntryValue_MultipleProviders()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Set3"));
            builder.AddProvider(new MemoryCacheProvider("Set4"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            Assert.False(cache.Contains(entry));

            cache.Set(entry);
            Assert.True(cache.Contains(entry));

            entry.Value = "Cabana";
            cache.Set(entry);
            Assert.True(cache.Contains(entry));

            object result = cache.Get(entry);
            Assert.Equal("Cabana", (string)result);
        }

        #endregion Set Tests

        #region Remove Tests

        /// <summary>
        /// Verifies removing a null entry returns null.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_RemoveEntry_NullEntry()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            object result = cache.Remove(null);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies an entry with a bad key returns null.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        public void CacheBuilderTest_RemoveEntry_BadKey(string key)
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            var entry = new CacheEntry(key);
            cache.Add(entry);
            object result = cache.Remove(entry);
            Assert.Null(result);
        }

        /// <summary>
        /// Verifies removing a missing entry returns null.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_RemoveEntry_MissingEntry_SingleProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            cache.Add(entry);
            Assert.Null(cache.Remove(new CacheEntry("Burger", "King")));
        }

        /// <summary>
        /// Verifies removing a missing entry returns null.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_RemoveEntry_MissingEntry_MultipleProviders()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider());
            builder.AddProvider(new MemoryCacheProvider("Remove"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            cache.Add(entry);
            Assert.Null(cache.Remove(new CacheEntry("Burger", "King")));
        }

        /// <summary>
        /// Verifies removing a valid entry returns the correct value.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_RemoveEntry_ValidEntry_SingleProvider()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Remover"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            cache.Add(entry);
            Assert.True(cache.Contains(entry));

            object result = cache.Remove(entry);
            Assert.Equal("Bell", (string)result);

            Assert.False(cache.Contains(entry));
        }

        /// <summary>
        /// Verifies removing a valid entry returns the correct value.
        /// </summary>
        [Fact]
        public void CacheBuilderTest_RemoveEntry_ValidEntry_MultipleProviders()
        {
            var builder = new CacheBuilder();
            builder.AddProvider(new MemoryCacheProvider("Remover2"));
            builder.AddProvider(new MemoryCacheProvider("Remover3"));
            ICache cache = builder;

            var entry = new CacheEntry("Taco", "Bell");
            cache.Add(entry);
            Assert.True(cache.Contains(entry));

            object result = cache.Remove(entry);
            Assert.Equal("Bell", (string)result);

            Assert.False(cache.Contains(entry));
        }

        #endregion Remove Tests
    }
}