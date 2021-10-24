namespace Oliviann
{
    #region Usings

    using System;
    using System.Diagnostics;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// disposable objects.
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        /// Disposes the of an <see cref="IDisposable"/> object by checking if
        /// it is <c>null</c> first.
        /// </summary>
        /// <param name="disposable">The disposable object.</param>
        /// <remarks>In C# 6 you can replace this extension method call with
        /// <c>DisposableObject?.Dispose()</c>.</remarks>
        [DebuggerStepThrough]
        public static void DisposeSafe(this IDisposable disposable) => disposable?.Dispose();
    }
}