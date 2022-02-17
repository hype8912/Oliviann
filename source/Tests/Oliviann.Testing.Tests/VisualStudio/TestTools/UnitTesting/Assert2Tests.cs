namespace Oliviann.Testing.Tests.VisualStudio.TestTools.UnitTesting
{
    #region Usings

    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Testing.VisualStudio.TestTools.UnitTesting;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    public class Assert2Tests
    {
        #region AreEqual Tests

        /// <summary>
        /// Verifies equal values do not throw an exception.
        /// </summary>
        [Fact]
        public void AreEqualTest_EqualValues()
        {
            var text = "Taco";
            Assert2.AreEqual("Taco", text, nameof(text));
        }

        /// <summary>
        /// Verifies an variable name does not throw an exception and the error
        /// message is formatted correctly.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("text")]
        public void AreEqualTest_NonEqual_VariableNames(string name)
        {
            string text = "Mfhbksf";

            try
            {
                Assert2.AreEqual("Taco", text, name);
            }
            catch (Exception ex)
            {
                Assert.Contains(" value is not correct.\n\tExpected:[Taco]\n\tActual:[Mfhbksf]", ex.Message);
            }
        }

        /// <summary>
        /// Verifies a custom message does not throw an exception and the
        /// error message is formatted correctly.
        /// </summary>
        [Theory]
        [InlineData(null, "text value is not correct.\n\tExpected:[Taco]\n\tActual:[Mfhbksf]")]
        [InlineData("", "text value is not correct.\n\tExpected:[Taco]\n\tActual:[Mfhbksf]")]
        [InlineData(" value is not correctly {0} formatted today.", "text value is not correctly Taco formatted today.\n\tExpected:[Taco]\n\tActual:[Mfhbksf]")]
        public void AreEqualTest_NonEqual_CustomMessages(string message, string expectedMessage)
        {
            string text = "Mfhbksf";

            try
            {
                Assert2.AreEqual("Taco", text, nameof(text), message);
            }
            catch (Exception ex)
            {
                Assert.Contains(expectedMessage, ex.Message);
            }
        }

        #endregion AreEqual Tests

        #region IsNull Tests

        /// <summary>
        /// Verifies no assertion exception is thrown.
        /// </summary>
        [Fact]
        public void IsNullTest_NullValue()
        {
            object testValue = null;
            Assert2.IsNull(testValue, nameof(testValue));
        }

        /// <summary>
        /// Verifies a non-null value throws assertion exception and the error
        /// message is correctly formatted.
        /// </summary>
        [Theory]
        [InlineData("text", "text object is not null.")]
        [InlineData("", " object is not null.")]
        [InlineData(null, " object is not null.")]
        public void IsNullTest_NonNullValue_VariableNames(string name, string expectedMessage)
        {
            string text = "Mfhbksf";

            try
            {
                Assert2.IsNull(text, name);
            }
            catch (Exception ex)
            {
                Assert.Contains(expectedMessage, ex.Message);
            }
        }

        /// <summary>
        /// Verifies a non-null value with a custom message throws assertion
        /// exception and the error message is correctly formatted.
        /// </summary>
        [Theory]
        [InlineData(null, "text object is not null.")]
        [InlineData("", "text object is not null.")]
        [InlineData(" why is this happening?", "text why is this happening?")]
        public void IsNullTest_NonNullValue_CustomMessages(string message, string expectedMessage)
        {
            string text = "Mfhbksf";

            try
            {
                Assert2.IsNull(text, nameof(text), message);
            }
            catch (Exception ex)
            {
                Assert.Contains(expectedMessage, ex.Message);
            }
        }

        #endregion IsNull Tests

        #region IsNotNull Tests

        /// <summary>
        /// Verifies a non-null value does not throw and exception.
        /// </summary>
        [Fact]
        public void IsNotNullTest_NonNullValue()
        {
            string text = "Mfhbksf";
            Assert2.IsNotNull(text, nameof(text));
        }

        /// <summary>
        /// Verifies a null value with variable throws assertion exception and
        /// the error message is correctly formatted.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("text")]
        public void IsNotNullTest_NullValue_VariableName(string name)
        {
            string text = null;

            try
            {
                Assert2.IsNotNull(text, name);
            }
            catch (Exception ex)
            {
                Assert.Contains(" object value is null.", ex.Message);
            }
        }

        /// <summary>
        /// Verifies a null value with a specific custom message throws an
        /// assertion exception and the error message is correctly formatted.
        /// </summary>
        [Theory]
        [InlineData(null, "text object value is null.")]
        [InlineData("", "text object value is null.")]
        [InlineData(" Why me! Why????", "text Why me! Why????")]
        public void IsNotNullTest_NullValue_MessageFormat(string customMessage, string expectedMessage)
        {
            string text = null;

            try
            {
                Assert2.IsNotNull(text, nameof(text), customMessage);
            }
            catch (Exception ex)
            {
                Assert.Contains(expectedMessage, ex.Message);
            }
        }

        #endregion IsNotNull Tests

        #region ThrowsException Tests

        /// <summary>
        /// Verifies an argument null exception is thrown when the action is
        /// null.
        /// </summary>
        [Fact]
        public void ThrowsExceptionTest_NullAction()
        {
            Assert.Throws<ArgumentNullException>(() => Assert2.ThrowsException<Exception>((Action)null));
        }

        /// <summary>
        /// Verifies an argument null exception is thrown when the action is
        /// null.
        /// </summary>
        [Fact]
        public void ThrowsExceptionTest_NullFunc()
        {
            Assert.Throws<ArgumentNullException>(() => Assert2.ThrowsException<Exception>(null));
        }

        [Fact]
        public void ThrowsExceptionTest_NoExceptionThrownWhenExpected()
        {
            Action act = () => { };
            Assert.Throws<AssertFailedException>(() => Assert2.ThrowsException<ArgumentException>(act, null));
        }

        [Fact]
        public void ThrowsExceptionTest_IncorrectExceptionThrown()
        {
            Action act = () =>
                {
                    string test = null;
                    ADP.CheckArgumentNull(test, "test");
                };

            Assert.Throws<AssertFailedException>(() => Assert2.ThrowsException<ArithmeticException>(act, null));
        }

        [Fact]
        public void ThrowsExceptionTest_ActionValidException()
        {
            Action act = () =>
                {
                    string test = null;
                    ADP.CheckArgumentNull(test, "test");
                };

            Assert2.ThrowsException<ArgumentNullException>(act, null);
        }

        [Fact]
        public void ThrowsExceptionTest_FuncValidException()
        {
            Func<object> act = () =>
                {
                    string test = null;
                    ADP.CheckArgumentNull(test, "test");
                    return null;
                };

            Assert2.ThrowsException<ArgumentNullException>(act, null);
        }

        #endregion
    }
}