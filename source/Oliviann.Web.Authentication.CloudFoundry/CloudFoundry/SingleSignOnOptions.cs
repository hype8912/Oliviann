namespace Oliviann.Web.Authentication.CloudFoundry
{
    /// <summary>
    /// Represents a model for the sign sign on options.
    /// </summary>
    public class SingleSignOnOptions
    {
        #region Properties

        /// <summary>
        /// Gets or sets the client url.
        /// </summary>
        public string ClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        #endregion
    }
}