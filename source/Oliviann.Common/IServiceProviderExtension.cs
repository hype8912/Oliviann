namespace Oliviann
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IServiceProvider"/> objects.
    /// </summary>
    public static class IServiceProviderExtension
    {
        /// <summary>Gets the service object of the specified type.</summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The service provider instance.</param>
        /// <returns>
        /// A service object of type <typeparamref name="T"/> or null if there
        /// is no service object of type <typeparamref name="T"/>.
        /// </returns>
        public static object GetService<T>(this IServiceProvider provider)
        {
            ADP.CheckArgumentNull(provider, nameof(provider));
            return provider.GetService(typeof(T));
        }
    }
}