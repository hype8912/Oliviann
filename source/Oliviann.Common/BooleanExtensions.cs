namespace Oliviann
{
    #region Usings

    using System.Diagnostics;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// boolean objects.
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// Converts the value of this instance to its equivalent string
        /// representation (either "Yes" or "No").
        /// </summary>
        /// <param name="boolean">The boolean value to be converted.</param>
        /// <returns>Yes if <c>true</c>; otherwise, No.</returns>
        [DebuggerStepThrough]
        public static string ToYesNoString(this bool boolean) => boolean ? @"Yes" : @"No";

        /// <summary>
        /// Converts the value in number format {1 , 0}.
        /// </summary>
        /// <param name="boolean">The boolean value to be converted.</param>
        /// <returns>1 if <c>true</c>; otherwise, 0.</returns>
        /// <example>
        ///     <code>
        ///         int result= default(bool).ToBit()
        ///     </code>
        /// </example>
        [DebuggerStepThrough]
        public static int ToBit(this bool boolean) => boolean ? 1 : 0;
    }
}