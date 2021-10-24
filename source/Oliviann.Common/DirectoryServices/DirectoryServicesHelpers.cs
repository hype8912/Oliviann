#if !NETSTANDARD1_3

namespace Oliviann.DirectoryServices
{
    #region Usings

    using System;
    using System.DirectoryServices.AccountManagement;
    using System.Security.Principal;

    #endregion Usings

    /// <summary>
    /// Represents a collection of helper methods for working with Active
    /// Directory.
    /// </summary>
    public static class DirectoryServicesHelpers
    {
        #region IsMemberOfGroup Methods

        /// <summary>
        /// Determines whether the current user is a member of the specified
        /// active directory group.
        /// </summary>
        /// <param name="groupName">Name of the directory group.</param>
        /// <returns>True if the current user is a member of the specified
        /// group; otherwise, false.</returns>
        /// <remarks>This method will not work for Local Domain groups where the
        /// user is in a different domain than the group.</remarks>
        public static bool IsMemberOfGroup(string groupName)
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            try
            {
                IPrincipal user = new WindowsPrincipal(identity);
                return user.IsInRole(groupName);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                identity.DisposeSafe();
            }
        }

        /// <summary>
        /// Determines whether the specified user is a member of the specified
        /// active directory group.
        /// </summary>
        /// <param name="groupName">Name of the directory group.</param>
        /// <param name="groupDomainName">Name of the domain the group is in.
        /// This is typically required for local domain groups.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userDomainName">Optional. Name of the domain the user
        /// is in if different from the group domain.</param>
        /// <returns>
        /// True if the specified user is a member of the specified group;
        /// otherwise, false.
        /// </returns>
        public static bool IsMemberOfGroup(string groupName, string groupDomainName, string userName, string userDomainName = null)
        {
            PrincipalContext groupDomainContext = null;
            PrincipalContext userDomainContext = null;
            UserPrincipal user = null;

            try
            {
                groupDomainContext = new PrincipalContext(ContextType.Domain, groupDomainName);
                userDomainContext = userDomainName.IsNullOrEmpty()
                                        ? groupDomainContext
                                        : new PrincipalContext(ContextType.Domain, userDomainName);

                user = UserPrincipal.FindByIdentity(userDomainContext, userName);
                if (user == null)
                {
                    // User's identity wasn't found in the specified domain.
                    return false;
                }

                using (GroupPrincipal group = GroupPrincipal.FindByIdentity(groupDomainContext, groupName))
                {
                    if (group == null)
                    {
                        // The group was not found in the specified domain.
                        return false;
                    }

                    return user.IsMemberOf(group);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                user.DisposeSafe();
                userDomainContext.DisposeSafe();
                groupDomainContext.DisposeSafe();
            }
        }

        #endregion IsMemberOfGroup Methods
    }
}

#endif