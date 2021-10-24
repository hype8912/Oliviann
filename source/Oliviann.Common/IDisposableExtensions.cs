namespace Oliviann
{
    #region Usings

    using System;
    using Oliviann.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a collection of extension methods for
    /// <see cref="IDisposable"/>.
    /// </summary>
    public static class IDisposableExtensions
    {
        #region Methods

        /// <summary>
        /// Disposes of an object safely.
        /// </summary>
        /// <param name="disposable">The first object to be disposed safely.
        /// </param>
        /// <param name="disposables">The array of other disposable objects to
        /// be disposed in order.</param>
        public static void DisposeSafe(this IDisposable disposable, params IDisposable[] disposables)
        {
            disposable?.Dispose();
            disposables.ForEach(d => d?.Dispose());
        }

        #endregion
    }
}