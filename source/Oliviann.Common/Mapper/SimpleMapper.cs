namespace Oliviann.Mapper
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Oliviann.Collections.Generic;
    using Oliviann.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a simple mapper class for mapping the values of the
    /// properties from one class to the properties of the exact same name of
    /// another class.
    /// </summary>
    public static class SimpleMapper
    {
        #region Methods

        /// <summary>
        /// Copies all the values of the properties of the source object to the
        /// properties in the destination object.
        /// </summary>
        /// <typeparam name="TSource">The type of object to be copied from.
        /// </typeparam>
        /// <typeparam name="TTarget">The type of object to be copied to.
        /// </typeparam>
        /// <param name="source">The source object instance to be read.</param>
        /// <param name="target">The destination object instance to be set.
        /// </param>
        /// <param name="ignoreCase">Optional. True to ignore case when
        /// comparing property names; otherwise, false. Default value is false.
        /// </param>
        public static void PropertyMap<TSource, TTarget>(TSource source, TTarget target, bool ignoreCase = false)
            where TTarget : class
        {
            if (source == null || target == null)
            {
                return;
            }

            PropertyInfo[] sourceProperties = typeof(TSource).GetProperties();
            PropertyInfo[] targetProperties = typeof(TTarget).GetProperties();
            StringComparison comparer = GetComparer(ignoreCase);

            PropertyMapInt(source, sourceProperties, target, targetProperties, comparer);
        }

        /// <summary>
        /// Copies all the values of the properties of the source collection
        /// objects to the properties of a new destination object.
        /// </summary>
        /// <typeparam name="TSource">The type of object to be copied from.
        /// </typeparam>
        /// <typeparam name="TTarget">The type of object to be copied to.
        /// </typeparam>
        /// <param name="source">The source object collection for values to be
        /// copied from.</param>
        /// <param name="ignoreCase">Optional. True to ignore case when
        /// comparing property names; otherwise, false. Default value is false.
        /// </param>
        /// <returns>
        /// A new collection of <typeparamref name="TTarget" /> objects.
        /// </returns>
        public static IEnumerable<TTarget> PropertyMap<TSource, TTarget>(IEnumerable<TSource> source, bool ignoreCase = false)
            where TTarget : class, new()
        {
            if (source.IsNullOrEmpty())
            {
                return Enumerable.Empty<TTarget>();
            }

            return PropertyMapInt<TSource, TTarget>(source, ignoreCase, source.Count());
        }

        /// <summary>
        /// Copies all the values of the properties of the source collection
        /// objects to the properties of a new destination object.
        /// </summary>
        /// <typeparam name="TSource">The type of object to be copied from.
        /// </typeparam>
        /// <typeparam name="TTarget">The type of object to be copied to.
        /// </typeparam>
        /// <param name="source">The source object collection for values to be
        /// copied from.</param>
        /// <param name="ignoreCase">Optional. True to ignore case when
        /// comparing property names; otherwise, false. Default value is false.
        /// </param>
        /// <returns>
        /// A new collection of <typeparamref name="TTarget" /> objects.
        /// </returns>
        public static IReadOnlyCollection<TTarget> PropertyMap<TSource, TTarget>(
            IReadOnlyCollection<TSource> source,
            bool ignoreCase = false)
            where TTarget : class, new()
        {
            if (source.IsNullOrEmpty())
            {
                return CollectionHelpers.CreateReadOnlyCollection<TTarget>();
            }

            return PropertyMapInt<TSource, TTarget>(source, ignoreCase, source.Count);
        }

        #endregion Methods

        #region Helper Methods

        private static void PropertyMapInt<TSource, TTarget>(
            TSource source,
            PropertyInfo[] sourceProperties,
            TTarget target,
            PropertyInfo[] targetProperties,
            StringComparison comparer)
        {
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo targetProperty =
                    targetProperties.FirstOrDefault(item => string.Equals(item.Name, sourceProperty.Name, comparer));

                if (targetProperty == null)
                {
                    continue;
                }

                try
                {
                    targetProperty.SetValue(
                        target,
                        targetProperty.PropertyType == typeof(string)
                            ? sourceProperty.GetValue(source).ToStringSafe()
                            : sourceProperty.GetValue(source));
                }
                catch (ArgumentException)
                {
                }
            }
        }

        private static IReadOnlyCollection<TTarget> PropertyMapInt<TSource, TTarget>(IEnumerable<TSource> source, bool ignoreCase, int initialCapacity)
            where TTarget : class, new()
        {
            PropertyInfo[] sourceProperties = typeof(TSource).GetProperties();
            PropertyInfo[] targetProperties = typeof(TTarget).GetProperties();
            StringComparison comparer = GetComparer(ignoreCase);

#if NET35 || NET40
            var target = new ReadOnlyListWrapper<TTarget>(initialCapacity);
#else
            var target = new List<TTarget>(initialCapacity);
#endif
            foreach (TSource sourceItem in source)
            {
                var targetItem = new TTarget();
                PropertyMapInt(sourceItem, sourceProperties, targetItem, targetProperties, comparer);

                target.Add(targetItem);
            }

            return target;
        }

        /// <summary>
        /// Determines the correct string comparer to be used.
        /// </summary>
        /// <param name="ignoreCase">True to ignore case when comparing property
        /// names; otherwise, false.</param>
        /// <returns>A string comparer type.</returns>
        private static StringComparison GetComparer(bool ignoreCase) =>
            ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        #endregion Helper Methods
    }
}