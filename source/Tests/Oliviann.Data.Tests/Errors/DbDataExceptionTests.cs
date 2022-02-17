namespace Oliviann.Data.Tests.Errors
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Security.Cryptography;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class DbDataExceptionTests
    {
        #region cTor Tests

        /// <summary>
        /// Verifies creating a new exception with a null message and a null
        /// errors collection is set correctly.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_NullMessage_NullErrors()
        {
            var ex = new DbDataException(null, errors: null);

            Assert.True(
                ex.Message == "Exception of type 'Oliviann.Data.DbDataException' was thrown.",
                "Exception message is incorrect.[{0}]".FormatWith(ex.Message));
            Assert.NotNull(ex.Errors);
            Assert.True(ex.Errors.Count == 0, "Errors collection is not empty.Count:[{0}]".FormatWith(ex.Errors.Count));
            Assert.True(ex.Source == string.Empty, "Exception source was incorrect.[{0}]".FormatWith(ex.Source));
            Assert.True(ex.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(ex.ErrorCode));
            Assert.True(ex.InnerException == null, "Inner exception is not null.");
        }

        /// <summary>
        /// Verifies creating a new exception with a message and no errors is
        /// set correctly.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_WithMessage_NoErrors()
        {
            var ex = new DbDataException("Exception message.", errors: null);

            Assert.True(ex.Message == "Exception message.", "Exception message is incorrect.[{0}]".FormatWith(ex.Message));
            Assert.NotNull(ex.Errors);
            Assert.True(ex.Errors.Count == 0, "Errors collection is not empty.Count:[{0}]".FormatWith(ex.Errors.Count));
            Assert.True(ex.Source == string.Empty, "Exception source was incorrect.[{0}]".FormatWith(ex.Source));
            Assert.True(ex.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(ex.ErrorCode));
            Assert.True(ex.InnerException == null, "Inner exception is not null.");
        }

        /// <summary>
        /// Verifies creating a new exception with a message and errors is set
        /// correctly.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_WithMessageAndErrors()
        {
            List<DbError> errors = this.GetRandomErrors(5);
            var ex = new DbDataException("exception message. Ah!!", errors);

            Assert.True(ex.Message == "exception message. Ah!!", "Exception message is incorrect.[{0}]".FormatWith(ex.Message));
            Assert.NotNull(ex.Errors);
            Assert.True(ex.Errors.Count == 5, "Errors collection is not empty.Count:[{0}]".FormatWith(ex.Errors.Count));
            Assert.True(ex.Source == "Error Source", "Exception source was incorrect.[{0}]".FormatWith(ex.Source));
            Assert.True(ex.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(ex.ErrorCode));
            Assert.True(ex.InnerException == null, "Inner exception is not null.");
        }

        /// <summary>
        /// Verifies creating a new exception with a null previous exception and
        /// a null inner exception throws a null reference exception.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_NullPrevious_NullInner()
        {
            DbDataException previous = null;
            ArgumentNullException inner = null;

            Assert.Throws<NullReferenceException>(() => new DbDataException(previous, inner));
        }

        /// <summary>
        /// Verifies creating a new exception with a valid previous exception
        /// and null inner exception is set correctly.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_ValidPrevious_NullInner()
        {
            var previous = new DbDataException("Previous message", inner: null);
            ArgumentNullException inner = null;

            var ex = new DbDataException(previous, inner);

            Assert.True(ex.Message == "Previous message", "Exception message is incorrect.[{0}]".FormatWith(ex.Message));
            Assert.NotNull(ex.Errors);
            Assert.True(ex.Errors.Count == 0, "Errors collection is not empty.Count:[{0}]".FormatWith(ex.Errors.Count));
            Assert.True(ex.Source == string.Empty, "Exception source was incorrect.[{0}]".FormatWith(ex.Source));
            Assert.True(ex.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(ex.ErrorCode));
            Assert.True(ex.InnerException == null, "Inner exception is not null.");
        }

        /// <summary>
        /// Verifies creating a new exception with a valid previous exception
        /// and inner exception is set correctly.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_ValidPrevious_ValidInner()
        {
            var previous = new DbDataException("Previous message", inner: null);
            var inner = new ArgumentNullException("someParam");

            var ex = new DbDataException(previous, inner);

            Assert.True(ex.Message == "Previous message", "Exception message is incorrect.[{0}]".FormatWith(ex.Message));
            Assert.NotNull(ex.Errors);
            Assert.True(ex.Errors.Count == 0, "Errors collection is not empty.Count:[{0}]".FormatWith(ex.Errors.Count));
            Assert.True(ex.Source == string.Empty, "Exception source was incorrect.[{0}]".FormatWith(ex.Source));
            Assert.True(ex.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(ex.ErrorCode));

            Assert.True(ex.InnerException != null, "Inner exception is null.");
            Assert.True(
                ex.InnerException.Message == "Value cannot be null.\r\nParameter name: someParam",
                "Inner exception message is not correct.[{0}]".FormatWith(ex.InnerException.Message));
        }

        /// <summary>
        /// Verifies creating a new exception with no inner exception is set
        /// correctly.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_WithMessage_NoInnerException()
        {
            var ex = new DbDataException("exception message", inner: null);

            Assert.True(ex.Message == "exception message", "Exception message is incorrect.[{0}]".FormatWith(ex.Message));
            Assert.NotNull(ex.Errors);
            Assert.True(ex.Errors.Count == 0, "Errors collection is not empty.Count:[{0}]".FormatWith(ex.Errors.Count));
            Assert.True(ex.Source == string.Empty, "Exception source was incorrect.[{0}]".FormatWith(ex.Source));
            Assert.True(ex.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(ex.ErrorCode));
            Assert.True(ex.InnerException == null, "Inner exception is not null.");
        }

        /// <summary>
        /// Verifies creating a new exception with a message and a inner
        /// exception is set correctly.
        /// </summary>
        [Fact]
        public void DbDataExceptionTest_WithMessageAndInnerException()
        {
            var inner = new ArgumentNullException("Something wrong.");
            var ex = new DbDataException("exception message. Ahoy!", inner);

            Assert.True(ex.Message == "exception message. Ahoy!", "Exception message is incorrect.[{0}]".FormatWith(ex.Message));
            Assert.NotNull(ex.Errors);
            Assert.True(ex.Errors.Count == 0, "Errors collection is not empty.Count:[{0}]".FormatWith(ex.Errors.Count));
            Assert.True(ex.Source == string.Empty, "Exception source was incorrect.[{0}]".FormatWith(ex.Source));
            Assert.True(ex.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(ex.ErrorCode));

            Assert.True(ex.InnerException != null, "Inner exception is null.");
            Assert.True(
                ex.InnerException.Message == "Value cannot be null.\r\nParameter name: Something wrong.",
                "Inner exception is incorrect.[{0}]".FormatWith(ex.InnerException.Message));
        }

        #endregion cTor Tests

        #region CreateException Tests

        /// <summary>
        /// Verifies an argument null exception is thrown when a null is passed
        /// in for errors.
        /// </summary>
        [Fact]
        public void CreateExceptionTest_NullErrors()
        {
            Assert.Throws<ArgumentNullException>(() => DbDataException.CreateException(null));
        }

        /// <summary>
        /// Verifies an empty collection doesn't throw any exceptions and
        /// returns back a new empty exception.
        /// </summary>
        [Fact]
        public void CreateExceptionTest_EmptyErrorsCollection()
        {
            var errors = new List<DbError>();
            DbDataException result = DbDataException.CreateException(errors);

            Assert.True(result.Message == string.Empty, "Exception message is incorrect.[{0}]".FormatWith(result.Message));
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.Count == 0, "Errors collection is not empty.Count:[{0}]".FormatWith(result.Errors.Count));
            Assert.True(result.Source == string.Empty, "Exception source was incorrect.[{0}]".FormatWith(result.Source));
            Assert.True(result.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(result.ErrorCode));
            Assert.True(result.InnerException == null, "Inner exception is not null.");
        }

        /// <summary>
        /// Verifies ...
        /// </summary>
        [Fact]
        public void CreateExceptionTest_WithErrors()
        {
            var errors = this.GetRandomErrors(5);
            DbDataException result = DbDataException.CreateException(errors);

            Assert.True(result.Message.Contains("[Broke] error message"), "Exception message is incorrect.[{0}]".FormatWith(result.Message));
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.Count == 5, "Errors collection is not empty.Count:[{0}]".FormatWith(result.Errors.Count));
            Assert.True(result.Source == "Error Source", "Exception source was incorrect.[{0}]".FormatWith(result.Source));
            Assert.True(result.ErrorCode == -2146232009, "ErrorCode value is incorrect.[{0}]".FormatWith(result.ErrorCode));
            Assert.True(result.InnerException == null, "Inner exception is not null.");
        }

        #endregion CreateException Tests

        #region Helper Methods

        private List<DbError> GetRandomErrors(uint count, string source = "Error Source")
        {
            var errors = new List<DbError>();
            var rand = new SecureRandom();

            for (uint i = 1; i <= count; i += 1)
            {
                int temp = rand.Next(0, int.MaxValue);
                errors.Add(new DbError(source, "error message" + temp, "Broke", temp));
            }

            return errors;
        }

        #endregion Helper Methods
    }
}