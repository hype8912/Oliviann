namespace Oliviann
{
    #region Usings

    using System;
    using System.Linq;
    using System.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// types.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Creates an instance of the specified type using that type's default
        /// constructor.
        /// </summary>
        /// <param name="objectType">The type of object to create.</param>
        /// <returns>Returns a newly created instance of the specified type.
        /// </returns>
        public static object CreateInstance(this Type objectType) => Activator.CreateInstance(objectType);

        /// <summary>
        /// Creates an instance of the specified type using that type's default
        /// constructor.
        /// </summary>
        /// <typeparam name="TOut">The output type of the newly created type.
        /// </typeparam>
        /// <param name="objectType">The type of object to create.</param>
        /// <returns>Returns a newly created instance of the specified type.
        /// </returns>
        public static TOut CreateInstance<TOut>(this Type objectType) => (TOut)objectType.CreateInstance();

        /// <summary>
        /// Creates an instance of the specified type using that type's default
        /// constructor.
        /// </summary>
        /// <typeparam name="TOut">The output type of the newly created type.
        /// </typeparam>
        /// <param name="objectType">The type of object to create.</param>
        /// <param name="args">An array of arguments that match in number,
        /// order, and type the parameters of the constructor to invoke. If args
        /// is an empty array or null, the constructor that takes no parameters
        /// (the default constructor) is invoked.</param>
        /// <returns>
        /// Returns a newly created instance of the specified type.
        /// </returns>
        public static TOut CreateInstance<TOut>(this Type objectType, params object[] args)
            => (TOut)Activator.CreateInstance(objectType, args);

#if NET40
        /// <summary>
        /// Returns the type information representation of the specified type.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The converted object.</returns>
        public static TypeDelegator GetTypeInfo(this Type type) => new TypeDelegator(type);
#endif

        /// <summary>
        /// Determines whether the type is nullable.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// True if the specified object type is nullable; otherwise, false.
        /// </returns>
        public static bool IsNullable(this Type objectType) => Nullable.GetUnderlyingType(objectType) != null;
    }
}