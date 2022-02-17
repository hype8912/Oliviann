#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a class for posting data to a web application.
    /// </summary>
    public class WebPost
    {
        /// <summary>
        /// Posts the data to Internet Explorer and then launching
        /// <paramref name="url"/>.
        /// </summary>
        /// <param name="url">The <paramref name="url"/> to launch once the data
        /// has been posted.</param>
        /// <param name="xmlText">The XML text to be posted to the
        /// <paramref name="url"/>.</param>
        /// <remarks>
        /// Doug feels there may be an intermittent issue using this method to
        /// launch Internet Explorer 8 on Windows Server 2008 R2.
        /// </remarks>
        public void ToInternetExplorer(string url, string xmlText)
        {
            Trace.WriteLine("Launching Internet Explorer...");
            object contentHeaders = @"Content-Type: application/x-www-form-urlencoded" + Environment.NewLine;
            object postData = Encoding.UTF8.GetBytes(xmlText);
            object empty = 0;
            object cc = true;

            Trace.WriteLine("Creating instance of Internet Explorer Application.");
            object browser = Type.GetTypeFromProgID(@"InternetExplorer.Application").CreateInstance();
            Type browserType = browser.GetType();

            Trace.WriteLine("Invoking Internet Explorer Navigate2 Method.\n    URL: " + url + "\n    DATA: " + xmlText);
            browserType.InvokeMember(@"Navigate2", BindingFlags.InvokeMethod, null, browser, new[] { url, empty, empty, postData, contentHeaders });

            Trace.WriteLine(@"Invoking Internet Explorer Visible Property to True.");
            browserType.InvokeMember(@"Visible", BindingFlags.SetProperty, null, browser, new[] { cc });
        }
    }
}

#endif