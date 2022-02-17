namespace Oliviann.Web.Services
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Oliviann.Collections.Generic;
    using Oliviann.Reflection;

    #endregion

    /// <summary>
    /// Represents a collection of helper methods for working with setting
    /// Reverse Proxy and WSSO header variables.
    /// </summary>
    internal static class HeaderVariablesHelpers
    {
        #region Methods

        /// <summary>
        /// Sets the specified types header variables in the specified headers
        /// collection.
        /// </summary>
        /// <typeparam name="TOut">The type of variables class.</typeparam>
        /// <param name="headers">The headers collection instance.</param>
        /// <param name="getHeaderValue">The delegate to retrieve the individual
        /// header value.</param>
        /// <returns>A new instance of the specified type with the available
        /// variables sets.</returns>
        internal static TOut SetHeaderVariables<TOut>(IEnumerable headers, Func<string, string> getHeaderValue) where TOut : new()
        {
            var variables = new TOut();
            if (headers.IsNullOrEmpty())
            {
                return variables;
            }

            IEnumerable<PropertyInfo> properties = typeof(TOut).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                string headerName = GetHeaderName(property);
                if (headerName == null)
                {
                    continue;
                }

                string headerValue = getHeaderValue(headerName);
                property.SetValue(variables, headerValue);
            }

            return variables;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the name of the header.
        /// </summary>
        /// <param name="property">The property to get the header name from.
        /// </param>
        /// <returns>A string for the header name.</returns>
        private static string GetHeaderName(PropertyInfo property)
        {
            var dataAttribute = property.GetCustomAttributeCached<DataMemberAttribute>();
            if (dataAttribute == null)
            {
                return null;
            }

            return string.IsNullOrEmpty(dataAttribute.Name) ? property.Name : dataAttribute.Name;
        }

        #endregion
    }
}