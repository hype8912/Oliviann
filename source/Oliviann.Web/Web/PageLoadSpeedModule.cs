#if NETFRAMEWORK

namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Text;
    using System.Web;

    #endregion Usings

    /// <summary>
    /// Represents a module for measuring page load time as a comment on the
    /// page source.
    /// </summary>
    public class PageLoadSpeedModule : BaseHttpModule
    {
        #region Fields

        /// <summary>
        /// Place holder for the stopwatch timer between events.
        /// </summary>
        private Stopwatch pageLoadTimer;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Starts the timer for measuring page load performance.
        /// </summary>
        /// <param name="context">The http base context.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the
        /// event data.</param>
        public override void OnPreRequestHandlerExecute(HttpContextBase context, EventArgs e)
        {
            this.pageLoadTimer = Stopwatch.StartNew();
        }

        /// <summary>
        /// Called when the post release request state event is raised.
        /// </summary>
        /// <param name="context">The http base context.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the
        /// event data.</param>
        public override void OnPostReleaseRequestState(HttpContextBase context, EventArgs e)
        {
            this.pageLoadTimer.Stop();
            if (!context.Response.ContentType.EqualsOrdinalIgnoreCase("text/html"))
            {
                return;
            }

            KeyValuePair<double, string> settings = GetSettingsMultiplier(ConfigurationManager.AppSettings["PageLoadModuleOutput"]);
            string result = BuildOutputString(settings, this.pageLoadTimer);

            context.Response.Write(result);
        }

        #endregion Methods

        #region Helper Methods

        /// <summary>
        /// Builds the output string from the specified settings and timer.
        /// </summary>
        /// <param name="settings">The settings pair. Key = multiplier; Value =
        /// name</param>
        /// <param name="timer">The stopped timer instance.</param>
        /// <returns>A compiled string to be written to the page.</returns>
        internal static string BuildOutputString(KeyValuePair<double, string> settings, Stopwatch timer)
        {
            var builder = new StringBuilder();
            builder.Append("<!-- Dynamic page generated in ");

            // Added to round the seconds to 3 decimal digits.
            if (settings.Value.EqualsOrdinalIgnoreCase("seconds"))
            {
                builder.Append(Math.Round(timer.ElapsedMilliseconds * settings.Key, 3));
            }
            else
            {
                builder.Append(timer.ElapsedMilliseconds * settings.Key);
            }

            builder.Append(" ");
            builder.Append(settings.Value);
            builder.Append(". -->");

            return builder.ToString();
        }

        /// <summary>
        /// Gets the page load settings output multiplier. The only two settings
        /// for the output is seconds and milliseconds with milliseconds being
        /// the default.
        /// </summary>
        /// <param name="setting">The settings value.</param>
        /// <returns>
        /// The correct multiplier and string for the timer output.
        /// </returns>
        internal static KeyValuePair<double, string> GetSettingsMultiplier(string setting)
        {
            return setting.EqualsOrdinalIgnoreCase("seconds")
                ? new KeyValuePair<double, string>(0.001, "seconds")
                : new KeyValuePair<double, string>(1D, "milliseconds");
        }

        #endregion Helper Methods
    }
}

#endif