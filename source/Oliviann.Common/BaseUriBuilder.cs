namespace Oliviann
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion Usings

    /// <summary>
    /// Represents a base class for a URI builder.
    /// </summary>
    public abstract class BaseUriBuilder
    {
        #region Fields

        /// <summary>
        /// Place holder for URL string parameters.
        /// </summary>
        private readonly IDictionary<string, string> parameters;

        /// <summary>
        /// Place holder for port number.
        /// </summary>
        private string _port;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUriBuilder"/>
        /// class.
        /// </summary>
        protected BaseUriBuilder()
        {
            this.parameters = new Dictionary<string, string>();
            this.HostUrl = this._port = string.Empty;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets or sets the base host URL.
        /// </summary>
        /// <value>The base host URL.</value>
        /// <example>
        /// <c>Example 1: http://myservername.com</c>
        /// <c>Example 2: http://192.168.1.1</c>
        /// <c>Example 3: http://servername.se.nos.boeing.com</c>
        /// </example>
        /// <remarks>Do not include the port number when setting the base URL value.</remarks>
        public string HostUrl { protected get; set; }

        /// <summary>
        /// Gets or sets the path to the resource referenced by the URI.
        /// </summary>
        /// <value>
        /// The path to the resource referenced by the URI.
        /// </value>
        public virtual string Path { protected get; set; }

        /// <summary>
        /// Gets or sets the port number of the application instance.
        /// </summary>
        /// <value>The port number.</value>
        public virtual int Port
        {
            protected get
            {
                return this._port.IsNullOrEmpty() ? -1 : this._port.ToInt32();
            }

            set
            {
                if (value.IsValidPort())
                {
                    this._port = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the URI for launching the DRS application.
        /// </summary>
        /// <value>The completed URI.</value>
        public virtual string Uri
        {
            get { return this.BuildBaseUrl() + this.BuildParametersString(); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a custom parameter and value if item is not available.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        /// <exception cref="ArgumentNullException">parameter can not be null.
        /// </exception>
        public void CustomParameter(string parameter, string value)
        {
            ADP.CheckArgumentNull(parameter, nameof(parameter));
            if (this.parameters.ContainsKey(parameter))
            {
                this.parameters[parameter] = value;
            }
            else
            {
                this.parameters.Add(parameter, value);
            }
        }

        #endregion Methods

        #region Overrides

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString() => this.Uri;

        #endregion Overrides

        #region Helper Methods

        /// <summary>
        /// Builds the base URL.
        /// </summary>
        /// <returns>The compiled base URL string.</returns>
        protected virtual string BuildBaseUrl()
        {
            return string.Concat(this.HostUrl, ":", this.Port.ToString(), "/", this.Path);
        }

        /// <summary>
        /// Joins the key value pair with an equal symbol and ends with a
        /// semi-colon.
        /// </summary>
        /// <param name="key">The string key.</param>
        /// <param name="value">The string value.</param>
        /// <returns>A concatenated key-value pair.</returns>
        private static string JoinPair(string key, string value)
        {
            return string.Concat(key, "=", value);
        }

        /// <summary>
        /// Builds the parameters string.
        /// </summary>
        /// <returns>Concatenated string of parameters for URL.</returns>
        private string BuildParametersString()
        {
            string link = this.parameters.Aggregate(
                                                    string.Empty,
                                                    (current, parm) => current + '&' + JoinPair(parm.Key, parm.Value));
            if (!link.IsNullOrEmpty())
            {
                link = '?' + link.Remove(0, 1);
            }

            return link;
        }

        #endregion Helper Methods
    }
}