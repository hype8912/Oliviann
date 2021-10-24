#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER

namespace Oliviann.Extensions.Hosting
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Extensions.Hosting;

    #endregion Usings

    /// <summary>
    /// Represents a collection of extension method for
    /// <see cref="IHostBuilder"/>.
    /// </summary>
    public static class IHostBuilderExtensions
    {
        #region Methods

        /// <summary>
        /// Allows for configuring application defaults easily.
        /// </summary>
        /// <param name="builder">The host builder instance.</param>
        /// <param name="act">The delegate to be executed.</param>
        /// <returns>The host builder instance.</returns>
        public static IHostBuilder ConfigureApplicationHostDefaults(this IHostBuilder builder, Action act)
        {
            if (builder is null)
            {
                return builder;
            }

            return builder;
        }

        #endregion Methods
    }
}

#endif