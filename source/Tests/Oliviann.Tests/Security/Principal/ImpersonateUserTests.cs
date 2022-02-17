namespace Oliviann.Tests.Security.Principal
{
    #region Usings

    using System;
    using System.ComponentModel;
    using System.IO;
    using Oliviann.Security;
    using Oliviann.Security.Principal;
    using Xunit;

    #endregion Usings

    public class ImpersonateUserTests
    {
        #region ctor Tests

        /// <summary>
        /// Verifies creating an instance of the class and then disposing
        /// doesn't throw an exception.
        /// </summary>
        [Fact]
        [Trait("Category", "CI")]
        public void ImpersonateUser_ctor_Create_Dispose()
        {
            var iu = new ImpersonateUser();
            iu.Dispose();
        }

        #endregion ctor Tests

        #region LogonUser Tests

        [Fact]
        [Trait("Category", "CI")]
        public void LogonUser_NullUsername_NullDomain_NullPassword()
        {
            Assert.Throws<Win32Exception>(
                () =>
                    {
                        var iu = new ImpersonateUser();

                        try
                        {
                            iu.LogonUser(null, null, null);
                        }
                        catch (Exception)
                        {
                            bool stop = true;
                            throw;
                        }
                        finally
                        {
                            iu.Dispose();
                        }
                    });
        }

        [Fact(Skip = "Ignore")]
        [Trait("Category", "Developer")]
        [Trait("Category", "SkipWhenLiveUnitTesting")]
        public void LogonUser_GoodUserName_GoodDomain_GoodPassword()
        {
            const string TestPath = @"\\ietm-fwb-web1.se.nos.boeing.com\c$\BGInfo\BGInfo.log";
            const string Username = @"svcfwbietm";
            const string Domain = @"se";

            // Developer must enter a password before running this test. DO NOT
            // check in any code with the password populated.
            const string Password = "";

            if (Password.IsNullOrEmpty())
            {
                throw new Exception("Developer needs to enter a password before executing test.");
            }

            bool firstResult = File.Exists(TestPath);
            Assert.False(firstResult);

            var iu = new ImpersonateUser();

            try
            {
                iu.LogonUser(Username, Domain, Password.ToSecureString());
                bool result = File.Exists(TestPath);
                Assert.True(result);
            }
            finally
            {
                iu.Dispose();
            }

            bool finalResult = File.Exists(TestPath);
            Assert.False(finalResult);
        }

        #endregion LogonUser Tests
    }
}