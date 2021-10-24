#if NET35

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Allows you to obtain the method or property name of the caller to the
    /// method. This is a trick for the compiler so it can be used with .NET
    /// 4.0 and lower.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerMemberNameAttribute : Attribute
    {
    }
}

#endif