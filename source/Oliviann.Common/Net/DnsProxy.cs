namespace Oliviann.Net
{
    #region Usings

    using System.Net;

    #endregion

    /// <summary>
    /// Represents a proxy class for calling the DNS class.
    /// </summary>
    public class DnsProxy : IDnsProxy
    {
        #region Methods

        /// <inheritdoc />
        public string GetHostName() => Dns.GetHostName();

        #endregion
    }
}