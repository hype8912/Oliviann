namespace Oliviann
{
    #region Usings

    using System;
    using Oliviann.Native;

    #endregion

    /// <summary>
    /// Represents a high resolution <see cref="DateTime"/> clock with the
    /// highest possible precision available.
    /// </summary>
    public static class HighResolutionDateTime
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes the <see cref="HighResolutionDateTime"/> class.
        /// </summary>
        static HighResolutionDateTime()
        {
            try
            {
                UnsafeNativeMethods.GetSystemTimePreciseAsFileTime(out _);
                IsAvailable = true;
            }
#if NETSTANDARD1_3
            catch (Exception)
            {
                IsAvailable = false;
            }
#else
            catch (EntryPointNotFoundException)
            {
                IsAvailable = false;
            }
#endif
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether high resolution time is available.
        /// </summary>
        /// <value>
        /// True if this instance is available; otherwise, false.
        /// </value>
        public static bool IsAvailable { get; }

        /// <summary>Gets a <see cref="DateTime" /> object that is set to the
        /// current date and time on this computer, expressed as the Coordinated
        /// Universal Time (UTC).</summary>
        /// <returns>An object whose value is the current UTC date and time.
        /// If high precision is not available, then the built in
        /// <see cref="DateTime"/> is called.
        /// </returns>
        public static DateTime UtcNow
        {
            get
            {
                if (!IsAvailable)
                {
                    return DateTime.UtcNow;
                }

                UnsafeNativeMethods.GetSystemTimePreciseAsFileTime(out long fileTime);
                return DateTime.FromFileTimeUtc(fileTime);
            }
        }

        #endregion
    }
}