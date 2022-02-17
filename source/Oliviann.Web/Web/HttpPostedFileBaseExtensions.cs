#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System.Web;
    using Oliviann.IO;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="HttpPostedFileBase"/>.
    /// </summary>
    public static class HttpPostedFileBaseExtensions
    {
        /// <summary>
        /// Gets the uploaded file as a binary array.
        /// </summary>
        /// <param name="postedFile">The posted file.</param>
        /// <returns>A binary array of the posted file.</returns>
        public static byte[] ToBinary(this HttpPostedFileBase postedFile)
        {
            ADP.CheckArgumentNull(postedFile, nameof(postedFile));
            return postedFile.InputStream.ToArray();
        }
    }
}

#endif