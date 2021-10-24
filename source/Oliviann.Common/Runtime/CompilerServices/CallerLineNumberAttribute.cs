#if NET35

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Allows you to obtain the line number in the source file at which the
    /// method is called. This is the file path at the time of compile. This is
    /// a trick for the compiler so it can be used with .NET 4.0 and lower.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerLineNumberAttribute : Attribute
    {
    }
}

#endif