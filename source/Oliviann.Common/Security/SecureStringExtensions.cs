namespace Oliviann.Security
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Security;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// secure strings.
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Indicates whether the specified secure string is a null reference or
        /// an empty secure string.
        /// </summary>
        /// <param name="secureText">The secure string object to test.</param>
        /// <returns>
        /// True if the <paramref name="secureText"/> parameter is a null
        /// reference or an empty string (""); otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this SecureString secureText)
        {
            return secureText == null || secureText.Length == 0;
        }

        /// <summary>
        /// Converts a <see cref="SecureString"/> object to an unsecured string.
        /// </summary>
        /// <param name="secureText">The secure string object to be converted.
        /// </param>
        /// <returns>
        /// A plain unsecured string for the specified secure string.
        /// </returns>
        public static string ToUnsecureString(this SecureString secureText)
        {
            if (secureText == null)
            {
                return null;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
#if NETSTANDARD1_3
                unmanagedString = SecureStringMarshal.SecureStringToGlobalAllocUnicode(secureText);
#else
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureText);
#endif
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Converts a plain unsecured string object into a secure string. It is
        /// recommended that once you convert the string to a secure string that
        /// you set the original string object to null.
        /// </summary>
        /// <param name="unsecureText">The unsecure text to be secured.</param>
        /// <returns>
        /// A new <see cref="SecureString"/> object representing the specified
        /// <paramref name="unsecureText"/> string.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "We returning the type so it needs to be disposed for by the calling method.")]
        public static SecureString ToSecureString(this string unsecureText)
        {
            ADP.CheckArgumentNull(unsecureText, "unsecuredText");
            SecureString secureText;

#if SAFE
            // The unsafe method is approximately 10x faster creating a
            // new object.
            secureText = new SecureString();
            foreach (char textChar in unsecureText)
            {
                secureText.AppendChar(textChar);
            }
#else
            unsafe
            {
                fixed (char* textChar = unsecureText)
                {
                    secureText = new SecureString(textChar, unsecureText.Length);
                }
            }
#endif

            secureText.MakeReadOnly();
            return secureText;
        }
    }
}