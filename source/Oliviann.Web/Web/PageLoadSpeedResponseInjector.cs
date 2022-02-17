#if NETSTANDARD2_0 || NETSTANDARD2_1
namespace Oliviann.Web
{
    #region Usings

    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    #endregion Usings

    /// <summary>
    /// Represents a class for injecting the elapsed milliseconds into the
    /// header for the request.
    /// </summary>
    public class PageLoadSpeedResponseInjector
    {
        #region Fields

        /// <summary>
        /// The current request delegate instance.
        /// </summary>
        private readonly RequestDelegate next;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PageLoadSpeedResponseInjector"/> class.
        /// </summary>
        /// <param name="next">The current request delegate instance.</param>
        public PageLoadSpeedResponseInjector(RequestDelegate next) => this.next = next;

        #endregion Constructor/Destructor

        #region Methods

        /// <summary>
        /// Executes the middleware operation to inject the response header.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <returns>An asynchronous task.</returns>
        public async Task Invoke(HttpContext context)
        {
            Stopwatch watch = Stopwatch.StartNew();

            // Add the header AFTER everything has executed.
            context.Response.OnStarting(
                state =>
                    {
                        var httpContext = (HttpContext)state;
                        httpContext.Response.Headers.Add("X-Response-Time-Milliseconds", new[] { watch.ElapsedMilliseconds.ToString() });

                        return Task.FromResult(0);
                    },
                context);

            await this.next(context);
        }

        #endregion Methods
    }
}
#endif