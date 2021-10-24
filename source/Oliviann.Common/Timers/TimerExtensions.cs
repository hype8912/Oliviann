#if !NETSTANDARD1_3

namespace Oliviann.Timers
{
    #region Usings

    using System;
    using System.Timers;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// Timer objects.
    /// </summary>
    public static class TimerExtensions
    {
        /// <summary>
        /// Resets the specified timer to start over again.
        /// </summary>
        /// <param name="timer">The timer object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="timer"/> is
        /// null.</exception>
        /// <remarks>Basically just stops the timer and then starts the timer.
        /// </remarks>
        public static void Reset(this Timer timer)
        {
            ADP.CheckArgumentNull(timer, nameof(timer));

            timer.Stop();
            timer.Start();
        }
    }
}

#endif