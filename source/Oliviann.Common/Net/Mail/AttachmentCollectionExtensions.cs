#if !NETSTANDARD1_3

namespace Oliviann.Net.Mail
{
    #region Usings

    using System.Net.Mail;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="AttachmentCollection"/>s.
    /// </summary>
    public static class AttachmentCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Creates a new <see cref="Attachment"/> instance with the specified
        /// path and adds it to the attachment collection.
        /// </summary>
        /// <param name="collection">The attachment collection to add a new
        /// attachment.</param>
        /// <param name="fileName">A string that contains a file path to use to
        /// create a new attachment.</param>
        public static void Add(this AttachmentCollection collection, string fileName)
        {
            ADP.CheckArgumentNull(collection, nameof(collection));
            ADP.CheckArgumentNullOrEmpty(fileName, nameof(fileName));

            collection.Add(new Attachment(fileName));
        }

        #endregion Methods
    }
}

#endif