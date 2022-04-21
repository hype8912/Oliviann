namespace Oliviann
{
    #region Usings

    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using Oliviann.Collections.Generic;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents an automatic data processing class for exceptions, checks,
    /// and errors.
    /// </summary>
    public static class ADP
    {
        #region Check

        /// <summary>
        /// Checks if the argument value is null or a
        /// <see cref="string.Empty"/> string and throws a
        /// <see cref="ArgumentNullException"/> if the specified string is null
        /// or a <see cref="string.Empty"/> string.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="parameterName">Name of the parameter or message to be
        /// displayed.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// null reference or <see cref="string.Empty"/> string.</exception>
        public static void CheckArgumentNullOrEmpty(string value, string parameterName, string message = null)
        {
            if (value.IsNullOrEmpty())
            {
                throw ArgumentNull(parameterName, message);
            }
        }

        /// <summary>
        /// Checks if the argument value is null or an empty collection and
        /// throws a <see cref="ArgumentNull"/> if null or an
        /// <see cref="ArgumentException"/> if empty.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is
        /// null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is
        /// empty.</exception>
        public static void CheckArgumentNullOrEmpty(IEnumerable value, string parameterName, string message = null)
        {
            ADP.CheckArgumentNull(value, parameterName, message);
            if (value.IsNullOrEmpty())
            {
                throw ArgumentEx(parameterName, message);
            }
        }

        /// <summary>
        /// Checks if the argument value is null and throws a
        /// <see cref="ArgumentNullException" /> if the specified object is
        /// null.
        /// </summary>
        /// <param name="value">The argument value.</param>
        /// <param name="parameterName">Name of the parameter or message to be
        /// displayed.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is
        /// null.</exception>
        public static void CheckArgumentNull(object value, string parameterName, string message = null)
        {
            if (value == null)
            {
                throw ArgumentNull(parameterName, message);
            }
        }

        /// <summary>
        /// Checks if the reference value is null and throws a
        /// <see cref="NullReferenceException"/> if the specified object is
        /// null.
        /// </summary>
        /// <param name="value">The reference value.</param>
        /// <param name="message">The null reference message.</param>
        /// <exception cref="NullReferenceException"><paramref name="value"/> is
        /// null.</exception>
        public static void CheckNullReference(object value, string message)
        {
            if (value == null)
            {
                throw NullReference(message);
            }
        }

        /// <summary>
        /// Checks if the specified file exists and throws a
        /// <see cref="FileNotFoundException"/> when the specified file could
        /// not be found or a disk failure occurred.
        /// </summary>
        /// <param name="filePath">The full path to the file to be checked.
        /// </param>
        /// <param name="fileName">The name of the file. The parameter is
        /// optional. This parameters is only used for reporting purposes. If
        /// the <paramref name="fileName"/> is null or string.Empty then the
        /// full specified <paramref name="filePath"/> will be returned in the
        /// exception; otherwise, just the
        /// <paramref name="fileName"/> will be returned in the exception.
        /// </param>
        /// <exception cref="FileNotFoundException">Thrown when the specified
        /// file path could not be found or a disk failure occurred.</exception>
        public static void CheckFileNotFound(string filePath, string fileName = null)
        {
            if (!File.Exists(filePath))
            {
                throw FileNotFound(fileName.IsNullOrEmpty() ? filePath : fileName);
            }
        }

        #endregion Check

        #region Exceptions

        /// <summary>
        /// Creates a new <see cref="ArgumentNullException" /> and ensures it's
        /// traced.
        /// </summary>
        /// <param name="parameter">The name of the parameter that caused the
        /// exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>
        /// A new <see cref="ArgumentNullException" />.
        /// </returns>
        public static ArgumentNullException ArgumentNull(string parameter, string message = null)
        {
            var ex = message == null
                         ? new ArgumentNullException(parameter)
                         : new ArgumentNullException(parameter, message);
            TraceException(ex);
            return ex;
        }

        /// <summary>
        /// Creates a new <see cref="ArgumentException" /> and ensures it's
        /// traced.
        /// </summary>
        /// <param name="parameter">The name of the parameter that caused the
        /// exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>
        /// A new <see cref="ArgumentException" />.
        /// </returns>
        public static ArgumentException ArgumentEx(string parameter, string message = null)
        {
            var ex = message == null
                ? new ArgumentException(parameter)
                : new ArgumentException(parameter, message);
            TraceException(ex);
            return ex;
        }

        /// <summary>
        /// Creates a new <see cref="FileNotFoundException"/> and ensures it's
        /// traced.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>A new <see cref="FileNotFoundException"/>.</returns>
        public static FileNotFoundException FileNotFound(string fileName)
        {
            var ex = new FileNotFoundException(Resources.ERR_FileNotFound.FormatWith(fileName), fileName);
            TraceException(ex);
            return ex;
        }

        /// <summary>
        /// Creates a new <see cref="InvalidOperationException"/> and ensures
        /// it's traced.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A new <see cref="InvalidOperationException"/>.</returns>
        public static InvalidOperationException InvalidOperation(string error)
        {
            var ex = new InvalidOperationException(error);
            TraceException(ex);
            return ex;
        }

        /// <summary>
        /// Creates a new <see cref="NullReferenceException"/> and ensures it's
        /// traced.
        /// </summary>
        /// <param name="message">The reference message.</param>
        /// <returns>A new <see cref="NullReferenceException"/>.</returns>
        public static NullReferenceException NullReference(string message)
        {
            var ex = new NullReferenceException(message);
            TraceException(ex);
            return ex;
        }

        #endregion Exceptions

        #region Trace

        /// <summary>
        /// Traces the specified exception message.
        /// </summary>
        /// <param name="ex">The exception object.</param>
        [DebuggerStepThrough]
        private static void TraceException(Exception ex) => Trace.TraceError(ex.Message);

        #endregion Trace
    }
}