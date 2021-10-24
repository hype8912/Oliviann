#if !NETSTANDARD1_3

namespace Oliviann.Security.Cryptography
{
#region Usings

    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Oliviann.Properties;

#endregion Usings

    /// <summary>
    /// Represents an implementation of the FIPS compliant Cryptography Next
    /// Generation (CNG) for working with hashes.
    /// </summary>
    public static class CryptoAlgorithms
    {
#region Enums

        /// <summary>
        /// The type of hashing algorithm to be used.
        /// </summary>
        public enum HashType
        {
            /// <summary>
            /// Specifies HMAC SHA1 hashing.
            /// </summary>
            /// <remarks>Uses FIPS compliant version.</remarks>
            HMACSHA1,

#if NETFRAMEWORK

            /// <summary>
            /// Specifies MAC using TripleDES hashing.
            /// </summary>
            [HashEngine(typeof(MACTripleDES))]
            MACTripleDES,

#endif

#if !ExcludeMD5
            /// <summary>
            /// Specifies MD5 hashing. No longer FIPS compliant.
            /// </summary>
            /// <remarks>Listed MD5 as condemned so support will be removed for
            /// MD5.
            /// </remarks>
#if NETFRAMEWORK

            [HashEngine(typeof(MD5Cng))]
#else
            [HashEngine(typeof(MD5CryptoServiceProvider))]
#endif
            [Obsolete("No longer considered secure. Migrate to SHA256 or higher.")]
            MD5,

#endif

            /// <summary>
            /// Specifies SHA1 hashing. No longer considered secure by NIST.
            /// </summary>
            /// <remarks>Uses FIPS compliant version. Do not use for anything
            /// that requires security.</remarks>
#if NETFRAMEWORK

            [HashEngine(typeof(SHA1Cng))]
#else
            [HashEngine(typeof(SHA1CryptoServiceProvider))]
#endif
            [Obsolete("Support for SHA1 will be removed in the future. Migrate to SHA256 or higher.")]
            SHA1,

            /// <summary>
            /// Specifies SHA-256 hashing.
            /// </summary>
            /// <remarks>Uses FIPS compliant version.</remarks>
#if NETFRAMEWORK

            [HashEngine(typeof(SHA256Cng))]
#else
            [HashEngine(typeof(SHA256CryptoServiceProvider))]
#endif
            SHA256,

            /// <summary>
            /// Default. Specifies SHA-384 hashing.
            /// </summary>
            /// <remarks>Uses FIPS compliant version.</remarks>
#if NETFRAMEWORK

            [HashEngine(typeof(SHA384Cng))]
#else
            [HashEngine(typeof(SHA384CryptoServiceProvider))]
#endif
            SHA384,

            /// <summary>
            /// Specifies SHA-512 hashing.
            /// </summary>
            /// <remarks>Uses FIPS compliant version.</remarks>
#if NETFRAMEWORK

            [HashEngine(typeof(SHA512Cng))]
#else
            [HashEngine(typeof(SHA512CryptoServiceProvider))]
#endif
            SHA512
        }

#endregion Enums

#region Compare Methods

        /// <summary>
        /// Compares the specified hash to the specified text the determine if
        /// that are a match.
        /// </summary>
        /// <param name="hash">The hash string to compare.</param>
        /// <param name="text">The text to be hashed and compared.</param>
        /// <param name="type">The type of hashing algorithm to be used.</param>
        /// <returns>True if the specified <paramref name="text"/> matches the
        /// <paramref name="hash"/>; otherwise, false.
        /// </returns>
        public static bool CompareHashes(string hash, string text, HashType type)
        {
            return CompareHashes(hash, text, type, Encoding.GetEncoding(0));
        }

        /// <summary>
        /// Compares the specified hash to the specified text the determine if
        /// that are a match.
        /// </summary>
        /// <param name="hash">The hash string to compare.</param>
        /// <param name="text">The text to be hashed and compared.</param>
        /// <param name="type">The type of hashing algorithm to be used.</param>
        /// <param name="enc">The character encoding type to be used for reading
        /// the input text.</param>
        /// <returns>True if the specified <paramref name="text"/> matches the
        /// <paramref name="hash"/>; otherwise, false.
        /// </returns>
        public static bool CompareHashes(string hash, string text, HashType type, Encoding enc)
        {
            string hash2 = ComputeHash(text, type, enc);
            return hash2 == hash;
        }

#endregion Compare Methods

#region Compute Methods

        /// <summary>
        /// Calculates the hash code for the specified text.
        /// </summary>
        /// <param name="text">The text to be hashed.</param>
        /// <param name="type">The type of hashing algorithm to be used.</param>
        /// <returns>
        /// A hashed string that represents the computed hash code.
        /// </returns>
        public static string ComputeHash(string text, HashType type) => ComputeHash(text, type, Encoding.ASCII);

        /// <summary>
        /// Calculates the hash code for the specified file path.
        /// </summary>
        /// <param name="filePath">The fully qualified file path to be hashed.
        /// </param>
        /// <param name="type">The type of hashing algorithm to be used.</param>
        /// <returns>A hashed string that represents the computed hash code.
        /// </returns>
        public static string ComputeFileHash(string filePath, HashType type)
        {
            HashAlgorithm algorithm = GetHashAlgorithm(type);
            return ComputeInternalFileHash(filePath, algorithm);
        }

        /// <summary>
        /// Calculates the hash code for the specified text using the specified
        /// encoding type.
        /// </summary>
        /// <param name="text">The text to be hashed.</param>
        /// <param name="type">The type of hashing algorithm to be used.</param>
        /// <param name="enc">The character encoding type to be used for reading
        /// the input text.</param>
        /// <returns>A hashed string that represents the computed hash code.
        /// </returns>
        public static string ComputeHash(string text, HashType type, Encoding enc)
        {
            if (enc == null)
            {
                enc = Encoding.ASCII;
            }

            HashAlgorithm algorithm = GetHashAlgorithm(type);
            return ComputeInternalHash(text, enc, algorithm);
        }

#endregion Compute Methods

#region Helper Methods

        /// <summary>
        /// Computes the hash value of the specified text.
        /// </summary>
        /// <param name="text">The input to compute hash for.</param>
        /// <param name="enc">The character encoding type to be used for reading
        /// the input text.</param>
        /// <param name="algorithm">The algorithm used for generating the hash.
        /// </param>
        /// <returns>The computed hash code for the specified string.</returns>
        private static string ComputeInternalHash(string text, Encoding enc, HashAlgorithm algorithm)
        {
            if (text == null)
            {
                return null;
            }

            byte[] buffer = enc.GetBytes(text);
            string hash;
            try
            {
                hash = BitConverter.ToString(algorithm.ComputeHash(buffer)).RemoveChar('-');
            }
            finally
            {
                algorithm.DisposeSafe();
            }

            return hash;
        }

        /// <summary>
        /// Computes the hash value for the specified file path.
        /// </summary>
        /// <param name="filePath">The input file path to computer hash for.
        /// </param>
        /// <param name="algorithm">The algorithm used for generating the hash.
        /// </param>
        /// <returns>The computed hash code for the specified file.</returns>
        private static string ComputeInternalFileHash(string filePath, HashAlgorithm algorithm)
        {
            if (filePath.IsNullOrEmpty())
            {
                return null;
            }

            FileStream stream = null;
            string hash;

            try
            {
                stream = File.OpenRead(filePath);
                hash = BitConverter.ToString(algorithm.ComputeHash(stream)).RemoveChar('-');
            }
            finally
            {
                stream.DisposeSafe();
                algorithm.DisposeSafe();
            }

            return hash;
        }

        /// <summary>
        /// Gets a new hash algorithm instance for the specified type.
        /// </summary>
        /// <param name="type">The algorithm used for generating the hash.
        /// </param>
        /// <returns>A new hash algorithm for the specified type.</returns>
        private static HashAlgorithm GetHashAlgorithm(HashType type)
        {
            switch (type)
            {
                case HashType.HMACSHA1:
                    return new HMACSHA1(Encoding.ASCII.GetBytes(Settings.Default.ASCII_Bytes), false);

                default:
                    Type algorithmType = type.GetAttribute<HashEngineAttribute>()?.HashType;
                    if (algorithmType == null)
                    {
#if NETFRAMEWORK
                        return new SHA384Cng();
#else
                        return new SHA384CryptoServiceProvider();
#endif
                    }

                    return algorithmType.CreateInstance<HashAlgorithm>();
            }
        }

#endregion Helper Methods
    }
}

#endif