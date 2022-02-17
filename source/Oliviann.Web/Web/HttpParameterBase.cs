#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    #endregion Usings

    /// <summary>
    /// Represents a base class for HTTP custom parameter injector
    /// implementations.
    /// </summary>
    public abstract class HttpParameterBase : BaseHttpModule
    {
        #region Methods

        /// <summary>
        /// Gets all the custom parameters values to be set.
        /// </summary>
        /// <param name="customParameterString">The custom parameter string.
        /// </param>
        /// <param name="getCustomParameterValue">The function for retrieving
        /// the customer parameter value based on the parameter key.</param>
        /// <returns>
        /// A collection of custom server variables and values.
        /// </returns>
#if DEBUG

        internal
#else
        protected
#endif
        static NameValueCollection GetCustomParameters(string customParameterString, Func<string, string> getCustomParameterValue)
        {
            if (customParameterString.IsNullOrWhiteSpace())
            {
                return new NameValueCollection();
            }

            var entries = new NameValueCollection();
            string[] customParameters = customParameterString.Split(StringSplitOptions.RemoveEmptyEntries, ';');
            foreach (string parameterKey in customParameters)
            {
                string parameterValue;
                try
                {
                    parameterValue = getCustomParameterValue(parameterKey);
                }
                catch (Exception ex)
                {
                    parameterValue = ex.Message;
                }

                entries.Add(parameterKey, parameterValue);
            }

            return entries;
        }

        /// <summary>
        /// Gets the parameter value for the specified key to be set.
        /// </summary>
        /// <param name="parameterKey">The parameter key to be set.
        /// </param>
        /// <returns>The string value for the specified key.</returns>s>
        protected virtual string GetParameterValue(string parameterKey) => ConfigurationManager.AppSettings[parameterKey];

        #endregion Methods
    }
}

#endif