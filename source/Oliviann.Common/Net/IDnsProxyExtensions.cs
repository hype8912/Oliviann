namespace Oliviann.Net
{
    #region Usings

    using System.Net.NetworkInformation;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="IDnsProxy"/> namespace easier.
    /// </summary>
    public static class IDnsProxyExtensions
    {
        /// <summary>
        /// Retrieves the full qualified domain name for the local machine.
        /// </summary>
        /// <param name="proxy">The DNS proxy instance.</param>
        /// <returns>The fully qualified domain name for the local machine.
        /// </returns>
        /// <remarks>Original code idea taken from the following link. <a>
        /// http://stackoverflow.com/a/804719/509719
        /// </a>
        /// </remarks>
        public static string GetFQDN(this IDnsProxy proxy)
        {
            ADP.CheckArgumentNull(proxy, nameof(proxy));
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = proxy.GetHostName();

            return hostName.Contains(domainName) ? hostName : hostName + "." + domainName;
        }
    }
}