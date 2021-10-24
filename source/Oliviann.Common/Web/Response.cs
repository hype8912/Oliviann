namespace Oliviann.Web
{
    /// <summary>
    /// Represents a generic API response model.
    /// </summary>
    public class Response<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        public int ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the response payload.
        /// </summary>
        public T Data { get; set; }

        #endregion Properties
    }
}