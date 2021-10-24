namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Oliviann.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// IEnumerable{T} objects.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Adds an object to the end of the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.
        /// </typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to add the
        /// <paramref name="item"/> to.</param>
        /// <param name="item">The object to be added to the end of the
        /// <see cref="IEnumerable{T}"/>. The value can be <c>null</c> for
        /// reference types.</param>
        /// <returns>
        /// A new <see cref="IEnumerable{T}"/> that contains elements from the
        /// input sequence and the specified <paramref name="item"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is
        /// a <c>null</c> reference.</exception>
        public static IEnumerable<TSource> Add<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            return new List<TSource>(source) { item };
        }

        /// <summary>
        /// Gets all the distinct items in the specified source using an
        /// optional comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">The property for determining distinctness.
        /// </param>
        /// <param name="comparer">Optional. The equality comparer for matching
        /// the keys.</param>
        /// <returns>A collection of unique items in the collection for the
        /// specified selector.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> selector,
            IEqualityComparer<TKey> comparer = null)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            ADP.CheckArgumentNull(selector, nameof(selector));

            var keys = new HashSet<TKey>(comparer);
            foreach (TSource item in source)
            {
                if (keys.Add(selector(item)))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="action"/> on each item in the
        /// collection.
        /// </summary>
        /// <typeparam name="T">The type of items in <paramref name="source"/>.
        /// </typeparam>
        /// <param name="source">The collection of items to perform a specific
        /// <paramref name="action"/> on.</param>
        /// <param name="action">The action to be performed on each collection
        /// item.</param>
        public static void Execute<T>(this IEnumerable<T> source, Action<T> action)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            ADP.CheckArgumentNull(action, nameof(action));

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Executes the specified <paramref name="action"/> on each item in the
        /// collection.
        /// </summary>
        /// <typeparam name="T">The type of items in <paramref name="source"/>.
        /// </typeparam>
        /// <param name="source">The collection of items to perform a specific
        /// <paramref name="action"/> on.</param>
        /// <param name="action">The action to be performed on each collection
        /// item.</param>
        /// <remarks>Provides another common name for the Execute extension
        /// method.</remarks>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) => source.Execute(action);

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is null
        /// and contains any elements.
        /// </summary>
        /// <param name="source">The collection of items to iterate over.
        /// </param>
        /// <returns>
        ///   <c>True</c> if the specified <paramref name="source"/> is empty;
        ///   otherwise,
        ///   <c>false</c>. Also will return <c>true</c> if the specified
        ///   <paramref name="source"/> is <c>null</c>.
        /// </returns>
        /// <remarks>I know this isn't the correct namespace but having it here
        /// keeps us from having conflicting namespace issues.</remarks>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            if (source == null)
            {
                return true;
            }

            if (source is ICollection collection)
            {
                return collection.Count == 0;
            }

            IEnumerator iterator = source.GetEnumerator();
            return !iterator.MoveNext();
        }

#if !NETSTANDARD1_3

        /// <summary>
        /// Converts the specified source collection to matching data table
        /// object.
        /// </summary>
        /// <typeparam name="T">The type of the elements of
        /// <paramref name="source" />.</typeparam>
        /// <param name="source">The source data collection of items.</param>
        /// <param name="tableName">The name to give the table. If tableName is
        /// null or an empty string, a default name is given.</param>
        /// <returns>
        /// A new data table object containing the same data as the source
        /// collection.
        /// </returns>
        public static System.Data.DataTable ToDataTable<T>(this IEnumerable<T> source, string tableName = null)
        {
            ADP.CheckArgumentNull(source, nameof(source));

            // Retrieve all the properties in the model type.
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            // Add in the columns to the data table from the model.
            var table = new System.Data.DataTable(tableName);
            foreach (PropertyDescriptor property in props)
            {
                table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            // Add all the property values to the table.
            var values = new object[props.Count];
            foreach (T item in source)
            {
                for (int i = 0; i < values.Length; i += 1)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            return table;
        }

#endif

        /// <summary>
        /// Creates a <see cref="List{T}"/> from an <see cref="IEnumerable{T}"/>
        /// by enumerating it asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.
        /// </typeparam>
        /// <param name="source">An IEnumerable to create a List{T} from.
        /// </param>
        /// <returns>A task that represents the asynchronous operation. The task
        /// result contains a <see cref="List{T}"/> that contains elements from
        /// the input sequence.</returns>
        public static System.Threading.Tasks.Task<List<T>> ToListAsync<T>(this IEnumerable<T> source)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            Func<List<T>> worker = () => source.ToList();

            return System.Threading.Tasks.Task.Factory.StartNew(worker);
        }
    }
}