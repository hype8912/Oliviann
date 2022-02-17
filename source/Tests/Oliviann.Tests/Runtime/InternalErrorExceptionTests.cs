namespace Oliviann.Tests.Runtime
{
    #region Usings

    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Oliviann.Runtime;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class InternalErrorExceptionTests
    {
        [Fact]
        public void InternalErrorExceptionTest_ctor_NullMessage()
        {
            var ex = new InternalErrorException(null);
            string result = ex.Message;

            Assert.Equal("Internal error occurred. Additional information: ''.", result);
        }

        [Fact]
        public void InternalErrorExceptionTest_ctor_StringMessage()
        {
            var ex = new InternalErrorException("Internal Error Exception");
            string result = ex.Message;

            Assert.Equal("Internal error occurred. Additional information: 'Internal Error Exception'.", result);
        }

        [Fact]
        public void InternalErrorExceptionTest_ctor_NullInfoDefaultContext()
        {
            Assert.Throws<ArgumentNullException>(() => new InternalErrorException(null, new StreamingContext()));
        }

        [Fact]
        public void InternalErrorExceptionTest_ctor_SerialInfo()
        {
            var ex = new InternalErrorException("Internal Error Exception");
            string exceptionToString = ex.ToString();

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Seek(0, 0);
                ex = (InternalErrorException)bf.Deserialize(ms);
            }

            Assert.Equal(exceptionToString, ex.ToString());
        }

        [Fact]
        public void InternalErrorExceptionTest_ctor_StringMessageAndNoInnerException()
        {
            var ex = new InternalErrorException("Internal Error Exception");
            string result = ex.Message;
            Assert.Equal("Internal error occurred. Additional information: 'Internal Error Exception'.", result);

            Exception inner = ex.InnerException;
            Assert.Null(inner);
        }
    }
}