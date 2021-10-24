#if NET35_OR_GREATER

namespace Oliviann.Security.Principal
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Principal;
    using Oliviann.Native;
    using Oliviann.Properties;
    using Oliviann.Win32.SafeHandles;

    #endregion Usings

    /// <summary>
    /// Represents a class for impersonating a Windows user on the local
    /// computer.
    /// </summary>
#if NET35
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
#endif

    public sealed class ImpersonateUser : IDisposable
    {
        #region Fields

        /// <summary>
        /// The Windows impersonation context object.
        /// </summary>
        private WindowsImpersonationContext context;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Executes the action as the specified user's account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The domain the user a member of.</param>
        /// <param name="password">The password associated with the user.
        /// </param>
        /// <param name="act">The action to be execute by the specified user.
        /// </param>
        public static void Execute(string userName, string domain, SecureString password, Action act)
        {
            ADP.CheckArgumentNull(act, nameof(act));

            using (var impersonate = new ImpersonateUser())
            {
                impersonate.LogonUser(userName, domain, password);
                act.Invoke();
            }
        }

        /// <summary>
        /// Create a logon token for executing under the specified user's
        /// credentials.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domain">The domain the user a member of.</param>
        /// <param name="password">The password associated with the user.
        /// </param>
        /// <exception cref="System.ComponentModel.Win32Exception">A user
        /// impersonation error has occurred.</exception>
        /// <remarks>
        /// Reference links:
        /// http://msdn.microsoft.com/en-us/library/w070t6ka%28v=vs.110%29.aspx
        /// http://www.codeproject.com/Articles/4051/Windows-Impersonation-using-C
        /// http://blogs.msdn.com/b/jimmytr/archive/2007/04/14/writing-test-code-with-impersonation.aspx
        /// http://www.cstruter.com/blog/270
        /// </remarks>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1113:CommaMustBeOnSameLineAsPreviousParameter", Justification = "Reviewed. Suppression is OK here.")]
        public void LogonUser(string userName, string domain, SecureString password)
        {
            bool impersonateSuccessful = UnsafeNativeMethods.LogonUser(
                userName,
                domain,
                password.ToUnsecureString(),
                LogonType.LOGON32_LOGON_INTERACTIVE,
                LogonProvider.LOGON32_PROVIDER_DEFAULT,
                out SafeTokenHandle currentUserToken);

            if (!impersonateSuccessful)
            {
                int errorCode = Marshal.GetLastWin32Error();
                Debug.WriteLine("LogonUser failed with error code: " + errorCode);
                currentUserToken?.Dispose();
                throw new Win32Exception(errorCode, Resources.ERR_Impersonation);
            }

            using (currentUserToken)
            using (var newId = new WindowsIdentity(currentUserToken.DangerousGetHandle()))
            {
                this.context = newId.Impersonate();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.context?.Undo();
            this.context?.Dispose();
        }

        #endregion Methods
    }
}

#endif