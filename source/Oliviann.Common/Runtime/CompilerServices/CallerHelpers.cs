namespace Oliviann.Runtime.CompilerServices
{
    #region Usings

    using System.Runtime.CompilerServices;

    #endregion

    /// <summary>
    /// Represents a collection of caller helper methods.
    /// </summary>
    public static class CallerHelpers
    {
        /// <summary>
        /// Gets the full path of the calling source file.
        /// </summary>
        /// <param name="filePath">The calling source file path.</param>
        /// <returns>The full path of the calling source file.</returns>
        public static string __FILE__([CallerFilePath] string filePath = "") => filePath;

        /// <summary>
        /// Gets the name of the calling function.
        /// </summary>
        /// <param name="memberName">Name of the calling function.</param>
        /// <returns>The name of the calling function.</returns>
        public static string __FUNC__([CallerMemberName] string memberName = "") => memberName;

        /// <summary>
        /// Gets the current line number of the calling function.
        /// </summary>
        /// <param name="lineNumber">The line number.</param>
        /// <returns>The current line number of the calling function.</returns>
        public static int __LINE__([CallerLineNumber] int lineNumber = 0) => lineNumber;
    }
}