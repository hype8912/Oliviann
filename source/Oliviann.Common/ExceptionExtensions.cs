namespace Oliviann
{
    #region Usings

    using System;
    using System.Collections.ObjectModel;
    using System.Text;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="Exception"/>s.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Aggregates all the messages and inner exception messages into a
        /// single string.
        /// </summary>
        /// <param name="ex">The top level exception to iterate.</param>
        /// <param name="separator">Optional. The separator between exception
        /// messages. Default value is a space.</param>
        /// <returns>A string containing all the messages from the current
        /// exception.</returns>
        public static string AggregateMessage(this Exception ex, string separator = " ")
        {
            var builder = new StringBuilder();
            while (ex != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }

                builder.Append(ex.Message);
                ex = ex.InnerException;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Adds a new <see cref="Exception" /> to the specified
        /// <see cref="AggregateException" /> collection.
        /// </summary>
        /// <param name="ae">The <see cref="AggregateException" /> reference.
        /// </param>
        /// <param name="ex">The <see cref="Exception" /> object to be added.
        /// </param>
        /// <returns>
        /// A new <see cref="AggregateException"/> collection.
        /// </returns>
        public static AggregateException AddNewException(this AggregateException ae, Exception ex)
        {
            if (ex == null)
            {
                return ae;
            }

            if (ae == null || ae.InnerExceptions.Count == 0)
            {
                return new AggregateException(ex);
            }

            var exceptions = new Collection<Exception> { ex };
            foreach (Exception innerEx in ae.InnerExceptions)
            {
                exceptions.Add(innerEx);
            }

            return new AggregateException(exceptions);
        }
    }
}