namespace Oliviann.Tests.DirectoryServices
{
    #region Usings

    using System;
    using System.Diagnostics;
    using Oliviann.DirectoryServices;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DirectoryServicesHelpersTests
    {
        /// <summary>
        /// Verifies the current running user identity is part of the specified
        /// active directory group.
        /// </summary>
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("&*^&TGUHGB*^gH&HNin08un)*N@~#!||", false)]
        [InlineData("kalsnkgasdglfmdsaf", false)]
        [InlineData("SOA_Share_C", true)]
        ////[InlineData("OPTIMUS_TEAM", true)]
        public void IsMemberOfTest_CurrentUser_GroupValues(string groupName, bool expectedResult)
        {
            bool result = DirectoryServicesHelpers.IsMemberOfGroup(groupName);
            Trace.WriteLine("User: " + Environment.UserName);
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Verifies the user specified is in the active directory group
        /// specified.
        /// </summary>
        /// <remarks>Some tests may fail if you aren't connected to the Oliviann
        /// network.</remarks>
        [Theory]
        [InlineData(null, "se", "b1545853", false)]
        [InlineData("", "se", "b1545853", false)]
        [InlineData("MDAO_Optimus_C", null, "b1545853", false)]
        [InlineData("MDAO_Optimus_C", "", "b1545853", false)]
        [InlineData("MDAO_Optimus_C", "se", "kdsnt452969342tng", false)]
        [InlineData("MDAO_Optimus_C", "se", null, false)]
        [InlineData("MDAO_Optimus_C", "se", "", false)]
        [InlineData("kalsnkgasdglfmdsaf", "se", "b1545853", false)]
        [InlineData("MDAO_Optimus_C", "mw", "b1545853", false)]
        ////[InlineData("OPTIMUS_TEAM", "se", "b1545853", true)]
        public void IsMemberOfTest_SpecifiedUser_GroupValues(string groupName, string groupDomain, string userName, bool expectedResult)
        {
            bool result = DirectoryServicesHelpers.IsMemberOfGroup(groupName, groupDomain, userName);
            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Verifies the user specified is in the active directory group
        /// specified in a different domain than the user.
        /// </summary>
        /// <remarks>This test will fail if you are not connected to the Oliviann
        /// network.</remarks>
        [Theory]
        [InlineData("APS_SOADEVTEAM", "sw", "b1545853", "se", true)]
        public void IsMemberOfTest_SpecifiedUser_GroupValues2(
            string groupName,
            string groupDomain,
            string userName,
            string userDomain,
            bool expectedResult)
        {
            bool result = DirectoryServicesHelpers.IsMemberOfGroup(groupName, groupDomain, userName, userDomain);
            Assert.Equal(expectedResult, result);
        }
    }
}