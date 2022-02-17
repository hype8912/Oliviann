#if NETFRAMEWORK

namespace Oliviann.Web.Tests
{
    #region Usings

    using System.IO;
    using System.Web;
    using System.Web.SessionState;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    public static class MockHelpers
    {
        /// <summary>
        /// Creates a new http context instance.
        /// </summary>
        /// <returns>A newly created http context instance.</returns>
        public static HttpContext CreateHttpContext()
        {
            var request = new HttpRequest(string.Empty, "http://localhost/", string.Empty);
            var writer = new StringWriter();
            var response = new HttpResponse(writer);
            var context = new HttpContext(request, response);

            var container = new HttpSessionStateContainer("id", new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 10, true, HttpCookieMode.AutoDetect, SessionStateMode.InProc, false);
            SessionStateUtility.AddHttpSessionStateToContext(context, container);

            return context;
        }
    }
}

#endif