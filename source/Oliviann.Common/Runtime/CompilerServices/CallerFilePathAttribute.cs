#if NET35

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Allows you to obtain the full path of the source file that contains the
    /// caller. This is the file path at the time of compile. This is a trick
    /// for the compiler so it can be used with .NET 4.0 and lower.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerFilePathAttribute : Attribute
    {
    }
}

#endif