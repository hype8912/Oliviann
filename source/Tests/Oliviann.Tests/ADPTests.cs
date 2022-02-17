namespace Oliviann.Tests
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ADPTests
    {
        #region CheckArgumentNullOrEmpty Tests

        /// <summary>
        /// Verifies an invalid string throws the correct exception.
        /// </summary>
        [Theory]
        [InlineData(null, "null")]
        [InlineData("", "null")]
        public void CheckArgumentNullOrEmptyTest_InvalidStrings(string value, string parmName)
        {
            Assert.Throws<ArgumentNullException>(() => ADP.CheckArgumentNullOrEmpty(value, parmName));
        }

        /// <summary>
        /// Verifies a null string with a message throws an exception.
        /// </summary>
        [Fact]
        public void CheckArgumentNullOrEmptyTest_NullStringWithMessage()
        {
            Assert.Throws<ArgumentNullException>(() => ADP.CheckArgumentNullOrEmpty(null, "null", "message"));
        }

        /// <summary>
        /// Verifies a valid string does not throw an exception.
        /// </summary>
        [Fact]
        public void CheckArgumentNullOrEmptyTest_ValidString()
        {
            ADP.CheckArgumentNullOrEmpty("null", "null");
        }

        /// <summary>
        /// Verifies a argument null exception is thrown for a null collection.
        /// </summary>
        [Fact]
        public void CheckCheckArgumentNullOrEmptyTest_NullCollection()
        {
            IEnumerable collection = null;
            Assert.Throws<ArgumentNullException>(() => ADP.CheckArgumentNullOrEmpty(collection, "collect", "message"));
        }

        /// <summary>
        /// Verifies a argument exception is thrown for an empty collection.
        /// </summary>
        [Fact]
        public void CheckCheckArgumentNullOrEmptyTest_EmptyCollection()
        {
            IEnumerable collection = Enumerable.Empty<string>();
            Assert.Throws<ArgumentException>(() => ADP.CheckArgumentNullOrEmpty(collection, "collect", "message"));
        }

        /// <summary>
        /// Verifies a valid collection doesn't throw an exception.
        /// </summary>
        [Fact]
        public void CheckCheckArgumentNullOrEmptyTest_ValidCollection()
        {
            IEnumerable collection = new List<string> { "787", "737" };
            ADP.CheckArgumentNullOrEmpty(collection, "collect", "message");
        }

        #endregion CheckArgumentNullOrEmpty Tests

        #region CheckArgumentNull Tests

        /// <summary>
        /// Verifies an null object throws the correct exception.
        /// </summary>
        [Fact]
        public void CheckArgumentNullTest_InvalidObjects()
        {
            Assert.Throws<ArgumentNullException>(() => ADP.CheckArgumentNull(null, "null"));
        }

        /// <summary>
        /// Verifies an empty string does not throw an exception.
        /// </summary>
        [Fact]
        public void CheckArgumentNullTest_EmptyString()
        {
            ADP.CheckArgumentNull(string.Empty, "null");
        }

        /// <summary>
        /// Verifies a null string with a message throws an exception.
        /// </summary>
        [Fact]
        public void CheckArgumentNullTest_NullStringWithMessage()
        {
            Assert.Throws<ArgumentNullException>(() => ADP.CheckArgumentNull(null, "null", "message"));
        }

        /// <summary>
        /// Verifies a valid string does not throw an exception.
        /// </summary>
        [Fact]
        public void CheckArgumentNullTest_ValidString()
        {
            ADP.CheckArgumentNull("tacos", "null");
        }

        #endregion CheckArgumentNull Tests

        #region CheckNullReference Tests

        /// <summary>
        /// Verifies passing a null object throws the correct exception.
        /// </summary>
        [Fact]
        public void CheckNullReferenceTest_NullObject()
        {
            Assert.Throws<NullReferenceException>(() => ADP.CheckNullReference(null, "Exception Message"));
        }

        /// <summary>
        /// Verifies passing an empty string does not throw an exception.
        /// </summary>
        [Fact]
        public void CheckNullReferenceTest_EmptyString()
        {
            ADP.CheckNullReference(string.Empty, "Exception Message");
        }

        /// <summary>
        /// Verifies passing a new instance does not throw an exception.
        /// </summary>
        [Fact]
        public void CheckNullReferenceTest_NewObject()
        {
            ADP.CheckNullReference(new object(), "Exception Message");
        }

        #endregion CheckNullReference Tests

        #region CheckFileNotFound Tests

        /// <summary>
        /// Verifies passing a null string throws the correct exception.
        /// </summary>
        [Fact]
        public void CheckFileNotFoundTest_NullString()
        {
            Assert.Throws<FileNotFoundException>(() => ADP.CheckFileNotFound(null));
        }

        /// <summary>
        /// Verifies passing an invalid string throws the correct exception.
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData(@"C:\Waba\Naba.ext")]
        public void CheckFileNotFoundTest_InvalidString(string path)
        {
            Assert.Throws<FileNotFoundException>(() => ADP.CheckFileNotFound(path));
        }

        /// <summary>
        /// Verifies passing a non-existent file path with the file name throws
        /// an exception.
        /// </summary>
        [Theory]
        [InlineData(@"C:\Waba\Naba.ext", "Naba.ext")]
        public void CheckFileNotFoundTest_MissingFileAndName(string path, string name)
        {
            Assert.Throws<FileNotFoundException>(() => ADP.CheckFileNotFound(path, name));
        }

        /// <summary>
        /// Verifies passing a valid file path does not throw an exception.
        /// </summary>
        [Fact]
        public void CheckFileNotFoundTest_ValidFilePath()
        {
            string filePath = @"C:\Windows\System32\cmd.exe";
            ADP.CheckFileNotFound(filePath);
        }

        #endregion CheckFileNotFound Tests

        #region ArgumentNull Tests

        /// <summary>
        /// Verifies passing a invalid parameter name returns the correct exception
        /// details.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ArgumentNullTest_InvalidParameter(string parm)
        {
            ArgumentNullException result = ADP.ArgumentNull(parm);

            Assert.NotNull(result);
            Assert.Equal("Value cannot be null.", result.Message);
        }

        /// <summary>
        /// Verifies passing an empty string parameter name with a custom
        /// message returns the correct exception details.
        /// </summary>
        [Fact]
        public void ArgumentNullTest_EmptyParameterWithCustomMessage()
        {
            const string ExpectedMessage = "This is a sample message.";
            ArgumentNullException result = ADP.ArgumentNull(string.Empty, ExpectedMessage);

            Assert.NotNull(result);
            Assert.Equal(result.Message, ExpectedMessage);
        }

        /// <summary>
        /// Verifies passing a valid string parameter name with a custom
        /// message returns the correct exception details.
        /// </summary>
        [Fact]
        public void ArgumentNullTest_ValidParameterWithCustomMessage()
        {
            ArgumentNullException result = ADP.ArgumentNull("argument9", "This is a sample message.");

            Assert.NotNull(result);
            Assert.Equal("This is a sample message.\r\nParameter name: argument9", result.Message);
        }

        /// <summary>
        /// Verifies passing a valid string parameter name returns the correct
        /// exception details.
        /// </summary>
        [Fact]
        public void ArgumentNullTest_ValidParameter()
        {
            ArgumentNullException result = ADP.ArgumentNull("argument9");

            Assert.NotNull(result);
            Assert.Equal("Value cannot be null.\r\nParameter name: argument9", result.Message);
        }

        #endregion ArgumentNull Tests

        // TODO: Need to write unit tests for FileNotFound.

        #region InvalidOperation Tests

        /// <summary>
        /// Verifies a null error string returns the correct exception details.
        /// </summary>
        [Fact]
        public void InvalidOperationTest_NullErrorString()
        {
            const string OriginalValue = null;
            InvalidOperationException result = ADP.InvalidOperation(OriginalValue);
            string shouldBeValue = "Exception of type 'System.InvalidOperationException' was thrown.";

            Assert.NotNull(result);
            Assert.Equal(result.Message, shouldBeValue);
        }

        /// <summary>
        /// Verifies an empty error string returns the correct exception
        /// details.
        /// </summary>
        [Fact]
        public void InvalidOperationTest_EmptyErrorString()
        {
            string originalValue = string.Empty;
            InvalidOperationException returnedValue = ADP.InvalidOperation(originalValue);
            string shouldBeValue = originalValue;

            Assert.NotNull(returnedValue);
            Assert.Equal(shouldBeValue, returnedValue.Message);
        }

        /// <summary>
        /// Verifies a valid error string returns the correct exception details.
        /// </summary>
        [Fact]
        public void InvalidOperationTest_ValidErrorString()
        {
            string originalValue = "Hello Message";
            InvalidOperationException returnedValue = ADP.InvalidOperation(originalValue);
            string shouldBeValue = originalValue;

            Assert.NotNull(returnedValue);
            Assert.Equal(shouldBeValue, returnedValue.Message);
        }

        #endregion InvalidOperation Tests

        // TODO: Need to write unit tests for NullReference.
    }
}