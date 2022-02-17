namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using System.Collections;
    using System.IO;
    using Oliviann.Collections.Generic;
    using Oliviann.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Testing.Fixtures;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [DeploymentItem(@"TestObjects\INI\AutoImports.ini")]
    [Trait("Category", "CI")]
    public class LineReaderTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly PathCleanupFixture fixture;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LineReaderTests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public LineReaderTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion Constructor/Destructor

        #region cTor Tests

        /// <summary>
        /// Verifies no exception is thrown when creating an instance.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(@"C:\Window-sakngkanfgla\Oliviann.txt")]
        [InlineData(@"C:\Windows\Oliviann-kdnga.txt")]
        public void LineReaderTest_FileName(string filename)
        {
            new LineReader(filename);
        }

        /// <summary>
        /// Verifies no exception is thrown when creating an instance with a
        /// valid file name.
        /// </summary>
        [Fact]
        public void LineReaderTest_ValidFileName()
        {
            string filename = Path.Combine(this.fixture.CurrentDirectory, @"TestObjects\INI\AutoImports.ini");
            new LineReader(filename);
        }

        #endregion cTor Tests

        #region GetEnumerator Tests

        /// <summary>
        /// Verifies passing in a null file name throws a argument null
        /// exception.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_NullFileName()
        {
            string filename = null;
            var reader = new LineReader(filename);

            int lineCount = 0;
            Assert.Throws<ArgumentNullException>(() => reader.ForEach(line => lineCount += 1));
        }

        /// <summary>
        /// Verifies passing in an empty file name throws a argument exception.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_EmptyFileName()
        {
            string filename = string.Empty;
            var reader = new LineReader(filename);

            int lineCount = 0;
            Assert.Throws<ArgumentException>(() => reader.ForEach(line => lineCount += 1));
        }

        /// <summary>
        /// Verifies passing in an invalid directory path throws a directory not
        /// found exception.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_InvalidDirectoryPath()
        {
            string filename = @"C:\Windows-hjgbjdsa\Oliviann.txt";
            var reader = new LineReader(filename);

            int lineCount = 0;
            Assert.Throws<DirectoryNotFoundException>(() => reader.ForEach(line => lineCount += 1));
        }

        /// <summary>
        /// Verifies passing in an invalid file name throws a file not found
        /// exception.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_InvalidFileName()
        {
            string filename = @"C:\Windows\OliviannXXXX.txt";
            var reader = new LineReader(filename);

            int lineCount = 0;
            Assert.Throws<FileNotFoundException>(() => reader.ForEach(line => lineCount += 1));
        }

        /// <summary>
        /// Verifies passing in a null function throws a exception.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_NullFunc()
        {
            Func<TextReader> func = null;
            var reader = new LineReader(func);

            int lineCount = 0;
            Assert.Throws<ArgumentNullException>(() => reader.ForEach(line => lineCount += 1));
        }

        /// <summary>
        /// Verifies passing in a null function throws a exception.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_NullFuncResult()
        {
            Func<TextReader> func = () => null;
            var reader = new LineReader(func);

            int lineCount = 0;
            Assert.Throws<NullReferenceException>(() => reader.ForEach(line => lineCount += 1));
        }

        /// <summary>
        /// Verifies a valid file iterates without error and returns the correct
        /// number of lines.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_ValidFileName()
        {
            string filename = Path.Combine(this.fixture.CurrentDirectory, @"TestObjects\INI\AutoImports.ini");
            var reader = new LineReader(filename);

            int lineCount = 0;
            foreach (string line in reader)
            {
                lineCount += 1;
            }

            Assert.Equal(69, lineCount);
        }

        /// <summary>
        /// Verifies a valid file iterates without error and returns the correct
        /// number of lines.
        /// </summary>
        [Fact]
        public void LineReaderGetEnumeratorTest_ValidFileName2()
        {
            string filename = Path.Combine(this.fixture.CurrentDirectory, @"TestObjects\INI\AutoImports.ini");
            var reader = (IEnumerable)new LineReader(filename);

            int lineCount = 0;
            foreach (object line in reader)
            {
                lineCount += 1;
            }

            Assert.Equal(69, lineCount);
        }

        #endregion GetEnumerator Tests
    }
}