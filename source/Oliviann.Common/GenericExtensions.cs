namespace Oliviann
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
#if !NETSTANDARD1_3
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using Oliviann.Properties;
#endif

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// generics.
    /// </summary>
    public static class GenericExtensions
    {
#if !NET35

        /// <summary>
        /// Creates a lazy computation that evaluates to the given function when
        /// required.
        /// </summary>
        /// <typeparam name="T">The type of input object.</typeparam>
        /// <param name="valueFactory">The function to be invoked when the value
        /// is needed.</param>
        /// <returns>A new lazy instance for retrieving the specified value.
        /// </returns>
        public static Lazy<T> AsLazy<T>(this Func<T> valueFactory) => new Lazy<T>(valueFactory);

        /// <summary>
        /// Creates a lazy computation that evaluates to the given value when
        /// required.
        /// </summary>
        /// <typeparam name="T">The type of input object.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>A new lazy instance for retrieving the specified value.
        /// </returns>
        public static Lazy<T> AsLazyFromValue<T>(this T value)
        {
            Func<T> function = () => value;
            return function.AsLazy();
        }

#endif

        /// <summary>
        /// Creates a shallow clone of the current instance by copying the
        /// references to a new object but not the reference objects.
        /// </summary>
        /// <typeparam name="T">The type of object being cloned.</typeparam>
        /// <param name="instance">The instance that implements the
        /// <see cref="ICloneable"/> interface.</param>
        /// <returns>A new object that is a copy of this instance.</returns>
        public static T CloneT<T>(this T instance) where T : ICloneable => (T)instance.Clone();

#if !NETSTANDARD1_3

        /// <summary>
        /// Creates a deep clone of the current instance by the use of binary
        /// serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being cloned. The specified
        /// type must be marked with the <see cref="SerializableAttribute"/> or
        /// an exception will be thrown.</typeparam>
        /// <param name="instance">The instance that implements the
        /// <see cref="ICloneable"/> interface.</param>
        /// <returns>A new object that is a copy of this instance.</returns>
        /// <exception cref="ArgumentException">The type must be serializable.
        /// </exception>
        public static T Copy<T>(this T instance) where T : class
        {
#if NET35 || NET40
            bool isSerializable = typeof(T).IsSerializable;
#else
            bool isSerializable = typeof(T).GetTypeInfo().IsSerializable;
#endif
            if (!isSerializable)
            {
                throw ADP.ArgumentEx(nameof(instance), Resources.ERR_TypeNotSerializable);
            }

            // Don't serialize a null object, simply return the default for
            // that object.
            if (instance == null)
            {
                return default;
            }

            MemoryStream stream = null;
            T result;

            try
            {
                stream = new MemoryStream();
                var formatter = new BinaryFormatter
                {
                    Context = new StreamingContext(StreamingContextStates.Clone),
                    TypeFormat = FormatterTypeStyle.TypesWhenNeeded
                };
                formatter.Serialize(stream, instance);
                stream.Seek(0, SeekOrigin.Begin);
                result = (T)formatter.Deserialize(stream);
            }
            finally
            {
                stream.DisposeSafe();
            }

            return result;
        }

#endif

        /// <summary>
        /// Returns a hash code for the specified object by determining if null
        /// first.
        /// </summary>
        /// <typeparam name="T">The type of object to get hash code for.
        /// </typeparam>
        /// <param name="value">The object to get hash code for.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing
        /// algorithms and data structures like a hash table.</returns>
        [DebuggerStepThrough]
        public static int GetSafeHashCode<T>(this T value) where T : class => value.GetValueOrDefault(v => v.GetHashCode(), 0);

        /// <summary>
        /// Retrieves the value of the specified action, or the specified
        /// default value.
        /// </summary>
        /// <typeparam name="TSource">The nullable type of the input.</typeparam>
        /// <typeparam name="TResult">The type of the output or result.</typeparam>
        /// <param name="source">The nullable source.</param>
        /// <param name="resultAction">The function to be performed if the
        /// specified source has a valid value.</param>
        /// <param name="defaultValue">The default value to be returned if the
        /// specified source does not have a valid value.</param>
        /// <returns>Returns the result of the specified result action if the
        /// specified source has a valid value; otherwise, the specified default
        /// value.</returns>
        public static TResult GetValueOrDefault<TSource, TResult>(
            this TSource? source,
            Func<TSource?, TResult> resultAction,
            TResult defaultValue)
            where TSource : struct => source.HasValue ? resultAction(source) : defaultValue;

        /// <summary>
        /// Retrieves the value of the specified action, or the specified
        /// default value.
        /// </summary>
        /// <typeparam name="TSource">The type of the input.</typeparam>
        /// <typeparam name="TResult">The type of the output or result.</typeparam>
        /// <param name="source">The source reference object.</param>
        /// <param name="resultAction">The function to be performed if the
        /// specified source is not null.</param>
        /// <param name="defaultValue">The default value to be returned if the
        /// specified source is null.</param>
        /// <returns>Returns the result of the specified result action if the
        /// specified source is not null; otherwise, the specified default
        /// value.</returns>
        public static TResult GetValueOrDefault<TSource, TResult>(
            this TSource source,
            Func<TSource, TResult> resultAction,
            TResult defaultValue)
            where TSource : class
        {
            return source == null ? defaultValue : resultAction(source);
        }

        /// <summary>
        /// Determines whether the specified value is equal to the default value.
        /// </summary>
        /// <typeparam name="T">The type of object being compared.</typeparam>
        /// <param name="value">The object value to be checked.</param>
        /// <returns>
        /// True if the specified value is the default value; otherwise, false.
        /// </returns>
        public static bool IsDefault<T>(this T value) => EqualityComparer<T>.Default.Equals(value, default(T));

        /// <summary>
        /// Puts a single object of type <typeparamref name="T"/> to a
        /// generic <see cref="List{T}"/> containing the specified
        /// <paramref name="obj"/>.
        /// </summary>
        /// <typeparam name="T">The type of object being inserted.</typeparam>
        /// <param name="obj">The object to be inserted into the collection.
        /// </param>
        /// <returns>A generic collection containing the specified
        /// <paramref name="obj"/>.</returns>
        [DebuggerStepThrough]
        public static List<T> PutList<T>(this T obj) => new() { obj };

        /// <summary>
        /// Executes a series of statements making repeated reference to a
        /// single object or structure.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">The object to perform the action on.</param>
        /// <param name="action">One or more statements that run on object.
        /// </param>
        public static void With<T>(this T obj, Action<T> action) => action(obj);
    }
}