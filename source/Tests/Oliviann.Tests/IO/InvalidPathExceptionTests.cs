namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Oliviann.IO;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class InvalidPathExceptionTests
    {
        [Fact]
        public void InvalidPathExceptionTest_ctor()
        {
            var ex = new InvalidPathException();
            string result = ex.Message;

            Assert.Equal("Exception of type 'Oliviann.IO.InvalidPathException' was thrown.", result);
        }

        [Fact]
        public void InvalidPathExceptionTest_ctor_NullMessage()
        {
            var ex = new InvalidPathException(null);
            string result = ex.Message;

            Assert.Equal("Exception of type 'Oliviann.IO.InvalidPathException' was thrown.", result);
        }

        [Fact]
        public void InvalidPathExceptionTest_ctor_StringMessage()
        {
            var ex = new InvalidPathException("File Path Exception");
            string result = ex.Message;

            Assert.Equal("File Path Exception", result);
        }

        [Fact]
        public void InvalidPathExceptionTest_ctor_NullInfoDefaultContext()
        {
            Assert.Throws<ArgumentNullException>(() => new InvalidPathException(null, new StreamingContext()));
        }

        [Fact]
        public void InvalidPathExceptionTest_ctor_SerialInfoAndNullException()
        {
            var ex = new InvalidPathException("File Path Exception", new Exception("Inner Exception."));
            string exceptionToString = ex.ToString();

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Seek(0, 0);
                ex = (InvalidPathException)bf.Deserialize(ms);
            }

            Assert.Equal(exceptionToString, ex.ToString());
        }

        [Fact]
        public void InvalidPathExceptionTest_ctor_StringMessageAndNullException()
        {
            var ex = new InvalidPathException("File Path Exception", null);
            string result = ex.Message;
            Assert.Equal("File Path Exception", result);

            Exception inner = ex.InnerException;
            Assert.Null(inner);
        }

        [Fact]
        public void InvalidPathExceptionTest_ctor_StringMessageAndInnerException()
        {
            var origInner = new ArgumentException("File path cannot be empty.");
            var ex = new InvalidPathException("File Path Exception", origInner);
            string result = ex.Message;
            Assert.Equal("File Path Exception", result);

            Exception inner = ex.InnerException;
            Assert.Equal(typeof(ArgumentException), inner.GetType());
            Assert.Equal("File path cannot be empty.", inner.Message);
        }
    }
}