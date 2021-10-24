namespace Oliviann.Net
{
    /// <summary>
    /// Represents a collection of commonly used helper methods for making
    /// working with <see cref="System.Net"/> namespace easier.
    /// </summary>
    public static class NetworkHelpers
    {
        /// <summary>
        /// Retrieves the full qualified domain name for the local machine.
        /// </summary>
        /// <returns>The fully qualified domain name for the local machine.
        /// </returns>
        /// <remarks>Original code idea taken from the following link. <a>
        /// http://stackoverflow.com/a/804719/509719
        /// </a>
        /// </remarks>
        public static string GetFQDN() => new DnsProxy().GetHostName();
    }
}