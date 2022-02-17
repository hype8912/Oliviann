namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Oliviann.IO;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class StreamExtensionsTests
    {
        #region ReadToEnd Tests

        /// <summary>
        /// Verifies calling a null stream throws an argument null exception.
        /// </summary>
        [Fact]
        public void ReadToEndTest_NullStream()
        {
            Stream st = null;
            Assert.Throws<ArgumentNullException>(() => st.ReadToEnd());
        }

        /// <summary>
        /// Verifies the text read from the stream is the same as it was
        /// written.
        /// </summary>
        [Fact]
        public void ReadToEndTest_ReadStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Hello world in C Version 1.6#.");
            writer.Flush();
            stream.Position = 0;

            string result = stream.ReadToEnd();
            writer.Close();
            stream.DisposeSafe();

            Assert.Equal("Hello world in C Version 1.6#.", result);
        }

        #endregion ReadToEnd Tests

        #region Write Tests

        /// <summary>
        /// Verifies calling a null stream throws an argument null exception.
        /// </summary>
        [Fact]
        public void StreamWriteTest_NullStream()
        {
            Stream st = null;
            Assert.Throws<ArgumentNullException>(() => st.Write("Hello"));
        }

        /// <summary>
        /// Verifies text is written to the stream without error.
        /// </summary>
        [Fact]
        public void StreamWriteTest_Text()
        {
            var stream = new MemoryStream();
            StreamWriter writer = stream.Write("Hello World!", false);

            stream.Position = 0;
            string result = stream.ReadToEnd();
            Assert.Equal("Hello World!", result);

            writer.DisposeSafe();
        }

        /// <summary>
        /// Verifies the writer is closed when auto close is set to true.
        /// </summary>
        [Fact]
        public void StreamWriteTest_AutoClose()
        {
            var stream = new MemoryStream();
            StreamWriter writer = stream.Write("Hello World!");

            GC.WaitForPendingFinalizers();
            Assert.Null(writer);
        }

        #endregion Write Tests

        #region ToStream Tests

        /// <summary>
        /// Verifies passing a null object to be converted to a stream returns a
        /// new stream.
        /// </summary>
        [Fact]
        public void ToStreamTest_NullObject()
        {
            object temp = null;
            MemoryStream stream = temp.ToStream();

            Assert.Null(stream);
        }

        /// <summary>
        /// Verifies converting a string to stream is the same at the original
        /// string when it's read back from the stream.
        /// </summary>
        [Fact]
        public void ToStreamTest_ValidString()
        {
            const string TestString = "Hello world in C Version 1.6#.";
            MemoryStream stream = TestString.ToStream();
            stream.Position = 0;
            object obj = new BinaryFormatter().Deserialize(stream);
            var result = obj as string;
            stream.Dispose();

            Assert.Equal(result, TestString);
        }

        #endregion ToStream Tests
    }
}