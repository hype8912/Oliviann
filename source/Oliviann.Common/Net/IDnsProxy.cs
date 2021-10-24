namespace Oliviann.Net
{
    #region Usings

    using System.Net;

    #endregion

    /// <summary>
    /// Represents an interface for implementing a Dns proxy.
    /// </summary>
    public interface IDnsProxy
    {
        /// <inheritdoc cref="Dns.GetHostName()"/>
        string GetHostName();
    }
}