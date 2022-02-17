namespace Oliviann.Testing.VisualStudio.TestTools.UnitTesting
{
    #region Usings

    using System;
    using System.Globalization;
    using Oliviann.Testing.Properties;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion Usings

    /// <summary>
    /// Represents a collection of methods to make writing unit tests easier.
    /// </summary>
    public static class Assert2
    {
        #region Fields

        /// <summary>
        /// The default incorrect message string.
        /// </summary>
        private const string DefaultIncorrectMessage = " value is not correct.";

        /// <summary>
        /// The default is null message string.
        /// </summary>
        private const string DefaultIsNullMessage = " object is not null.";

        /// <summary>
        /// The default is not null message string.
        /// </summary>
        private const string DefaultIsNotNullMessage = " object value is null.";

        #endregion Fields

        /// <summary>
        /// Verifies that two specified generic type data are equal by using the
        /// equality operator. The assertion fails if they are not equal.
        /// Displays a message if the assertion fails, and applies the specified
        /// formatting to it.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <param name="expectedValue">The first generic type data to compare.
        /// This is the generic type data the unit test expects.</param>
        /// <param name="actualValue">The second generic type data to compare.
        /// This is the generic type data the unit test produced.</param>
        /// <param name="variableName">The name of the variable to be added to
        /// the message.</param>
        /// <param name="message">Optional. A message to display if the
        /// assertion fails. This message can be seen in the unit test results
        /// with the variable name, expected value, and actual value appended.
        /// </param>
        public static void AreEqual<T>(T expectedValue, T actualValue, string variableName, string message = DefaultIncorrectMessage)
        {
            if (message.IsNullOrEmpty())
            {
                message = DefaultIncorrectMessage;
            }

            string errorMessage = variableName + message + "\n\tExpected:[{0}]\n\tActual:[{1}]";
            Assert.AreEqual(expectedValue, actualValue, errorMessage, expectedValue, actualValue);
        }

        /// <summary>
        /// Verifies the specified object is null. Displays a message if the
        /// assertion fails.
        /// </summary>
        /// <param name="value">The object to verify is null.</param>
        /// <param name="variableName">The name of the variable to be added to
        /// the message.</param>
        /// <param name="message">Optional. A message to display if the
        /// assertion fails. This message can be seen in the unit test results
        /// with the variable name appended.</param>
        public static void IsNull(object value, string variableName, string message = DefaultIsNullMessage)
        {
            if (message.IsNullOrEmpty())
            {
                message = DefaultIsNullMessage;
            }

            string errorMessage = variableName + message;
            Assert.IsNull(value, errorMessage);
        }

        /// <summary>
        /// Verifies the specified object is not null. Displays a message if the
        /// assertion fails.
        /// </summary>
        /// <param name="value">The object to verify is not null.</param>
        /// <param name="variableName">The name of the variable to be added to
        /// the message.</param>
        /// <param name="message">Optional. A message to display if the
        /// assertion fails. This message can be seen in the unit test results
        /// with the variable name appended.</param>
        public static void IsNotNull(object value, string variableName, string message = DefaultIsNotNullMessage)
        {
            if (message.IsNullOrEmpty())
            {
                message = DefaultIsNotNullMessage;
            }

            string errorMessage = variableName + message;
            Assert.IsNotNull(value, errorMessage);
        }

        #region ThrowsException

        /// <summary>
        /// Tests whether the code specified by delegate
        /// <paramref name="action"/> throws exact given exception of type
        /// <typeparamref name="T"/> (and not of derived type) and throws a
        /// failed if code does not throws exception or throws exception of type
        /// other than <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of exception expected to be thrown.
        /// </typeparam>
        /// <param name="action">
        /// Delegate to code to be tested and which is expected to throw
        /// exception.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when
        /// <paramref name="action"/>does not throws exception of type
        /// <typeparamref name="T"/>.
        /// </param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(Action action, string message = "") where T : Exception
        {
            return ThrowsException<T>(action, message, null);
        }

        /// <summary>
        /// Tests whether the code specified by delegate
        /// <paramref name="action"/> throws exact given exception of type
        /// <typeparamref name="T"/> (and not of derived type) and throws a
        /// failed if code does not throws exception or throws exception of type
        /// other than <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of exception expected to be thrown.
        /// </typeparam>
        /// <param name="action">
        /// Delegate to code to be tested and which is expected to throw
        /// exception.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when
        /// <paramref name="action"/>does not throws exception of type
        /// <typeparamref name="T"/>.
        /// </param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(Func<object> action, string message = "")
            where T : Exception
        {
            return ThrowsException<T>(action, message, null);
        }

        /// <summary>
        /// Tests whether the code specified by delegate
        /// <paramref name="action"/> throws exact given exception of type
        /// <typeparamref name="T"/> (and not of derived type) and throws a
        /// failed if code does not throws exception or throws exception of type
        /// other than <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of exception expected to be thrown.
        /// </typeparam>
        /// <param name="action">
        /// Delegate to code to be tested and which is expected to throw
        /// exception.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when
        /// <paramref name="action"/>does not throws exception of type
        /// <typeparamref name="T"/>.
        /// </param>
        /// <param name="parameters">
        /// An array of parameters to use when formatting
        /// <paramref name="message"/>.
        /// </param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(Func<object> action, string message, params object[] parameters)
            where T : Exception
        {
            ADP.CheckArgumentNull(action, nameof(action));
            return ThrowsException<T>(() => { action(); }, message, parameters);
        }

        /// <summary>
        /// Tests whether the code specified by delegate
        /// <paramref name="action"/> throws exact given exception of type
        /// <typeparamref name="T"/> (and not of derived type) and throws a
        /// failed if code does not throws exception or throws exception of type
        /// other than <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of exception expected to be thrown.
        /// </typeparam>
        /// <param name="action">
        /// Delegate to code to be tested and which is expected to throw
        /// exception.
        /// </param>
        /// <param name="message">
        /// The message to include in the exception when
        /// <paramref name="action"/>does not throws exception of type
        /// <typeparamref name="T"/>.
        /// </param>
        /// <param name="parameters">
        /// An array of parameters to use when formatting
        /// <paramref name="message"/>.
        /// </param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(Action action, string message, params object[] parameters)
            where T : Exception
        {
            ADP.CheckArgumentNull(action, nameof(action));
            if (message == null)
            {
                message = string.Empty;
            }

            string finalMessage;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (typeof(T) != ex.GetType())
                {
                    finalMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.WrongExceptionThrown,
                        message,
                        typeof(T).Name,
                        ex.GetType().Name,
                        ex.Message,
                        ex.StackTrace);
                    Assert.Fail(finalMessage, parameters);
                }

                return (T)ex;
            }

            // No exception thrown.
            finalMessage = string.Format(CultureInfo.CurrentCulture, Resources.NoExceptionThrown, message, typeof(T).Name);
            Assert.Fail(finalMessage, parameters);
            return null;
        }

        #endregion
    }
}