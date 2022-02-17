namespace Oliviann.Tests.IO
{
    #region Usings

    using System;
    using Oliviann.IO;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class FileSizeTests
    {
        #region Constructor Tests

        [Fact]
        public void FileSizeConstructorTest_TestDefaultValues()
        {
            var fs = new FileSize();

            Assert.Equal(0UL, fs.Bytes);
            Assert.Equal(0D, fs.Kilobytes);
            Assert.Equal(0D, fs.Megabytes);
            Assert.Equal(0D, fs.Gigabytes);
            Assert.Equal(0D, fs.Terabytes);
            Assert.Equal(0D, fs.Petabytes);
            Assert.Equal(0D, fs.Exabytes);
        }

        ////[Fact]
        ////public void FileSizeConstructorTest_SmallNegativeInt()
        ////{
        ////    var fs = new FileSize(-99);

        ////    Assert.Equal(-99L, fs.Bytes);
        ////    Assert.Equal(-0.0966796875, fs.Kilobytes);
        ////    Assert.Equal(-9.44138E-05, fs.Megabytes);
        ////    Assert.Equal(-9.22E-08, fs.Gigabytes);
        ////    Assert.Equal(-1E-10, fs.Terabytes);
        ////    Assert.Equal(0D, fs.Petabytes);
        ////    Assert.Equal(0D, fs.Exabytes);
        ////}

        ////[Fact]
        ////public void FileSizeConstructorTest_LargeNegativeInt()
        ////{
        ////    var fs = new FileSize(-8484945920);

        ////    Assert.Equal(-8484945920L, fs.Bytes);
        ////    Assert.Equal(-8286080, fs.Kilobytes);
        ////    Assert.Equal(-8091.875, fs.Megabytes);
        ////    Assert.Equal(-7.9022216797, fs.Gigabytes);
        ////    Assert.Equal(-0.0077170134, fs.Terabytes);
        ////    Assert.Equal(-7.5361E-06, fs.Petabytes);
        ////    Assert.Equal(-7.4E-09, fs.Exabytes);
        ////}

        #endregion Constructor Tests

        #region Compare Tests

        [Fact]
        public void FileSizeCompareToTest_DoubleZero()
        {
            var fs1 = new FileSize();
            var fs2 = new FileSize();

            int result = fs1.CompareTo(fs2);
            Assert.Equal(0, result);
        }

        [Fact]
        public void FileSizeCompareToTest_LargerLeftRightZero()
        {
            var fs1 = new FileSize(99);
            var fs2 = new FileSize();

            int result1 = fs1.CompareTo(fs2);
            Assert.Equal(1, result1);

            int result2 = fs2.CompareTo(fs1);
            Assert.Equal(-1, result2);
        }

        [Fact]
        public void FileSizeCompareToObjectTest_Null()
        {
            var fs = FileSize.FromMegabytes(2048);
            object obj = null;

            int result = fs.CompareTo(obj);
            Assert.Equal(1, result);
        }

        [Fact]
        public void FileSizeCompareToObjectTest_Object()
        {
            var fs = FileSize.FromMegabytes(2048);
            var obj = new object();

            Assert.Throws<ArgumentException>(() => fs.CompareTo(obj));
        }

        [Fact]
        public void FileSizeCompareToObjectTest_True()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048) as object;

            int result = fs1.CompareTo(fs2);
            Assert.Equal(0, result);
        }

        #endregion Compare Tests

        #region Static Equals Tests

        [Fact]
        public void FileSizeStaticEqualsTest_EqualSizes()
        {
            var fs1 = new FileSize();
            var fs2 = new FileSize();

            bool result = FileSize.Equals(fs1, fs2);
            Assert.True(result, "Equals sizes not found to be correct.");

            fs1 = new FileSize(100);
            fs2 = new FileSize(100);

            bool result2 = FileSize.Equals(fs1, fs2);
            Assert.True(result2, "Equals sizes not found to be correct.");
        }

        [Fact]
        public void FileSizeStaticEqualsTest_NotEqualSizes()
        {
            var fs1 = new FileSize(100);
            var fs2 = new FileSize();

            bool result = FileSize.Equals(fs1, fs2);
            Assert.False(result, "Non-Equals sizes not found to be correct.");

            fs1 = new FileSize(100);
            fs2 = new FileSize(200);

            bool result2 = FileSize.Equals(fs1, fs2);
            Assert.False(result2, "Non-Equals sizes not found to be correct.");
        }

        #endregion Static Equals Tests

        #region From Tests

        [Fact]
        public void FileSizeFromBytesTest()
        {
            var fs = FileSize.FromBytes(2147483648);

            Assert.Equal(2147483648UL, fs.Bytes);
            Assert.Equal(2097152D, fs.Kilobytes);
            Assert.Equal(2048D, fs.Megabytes);
            Assert.Equal(2D, fs.Gigabytes);
            Assert.Equal(0.001953125D, fs.Terabytes);
            Assert.Equal(1.9073E-06D, fs.Petabytes);
            Assert.Equal(1.9E-09D, fs.Exabytes);
        }

        [Fact]
        public void FileSizeFromKilobytesTest()
        {
            var fs = FileSize.FromKilobytes(2097152);

            Assert.Equal(2147483648UL, fs.Bytes);
            Assert.Equal(2097152D, fs.Kilobytes);
            Assert.Equal(2048D, fs.Megabytes);
            Assert.Equal(2D, fs.Gigabytes);
            Assert.Equal(0.001953125D, fs.Terabytes);
            Assert.Equal(1.9073E-06D, fs.Petabytes);
            Assert.Equal(1.9E-09D, fs.Exabytes);
        }

        [Fact]
        public void FileSizeFromMegabytesTest()
        {
            var fs = FileSize.FromMegabytes(2048);

            Assert.Equal(2147483648UL, fs.Bytes);
            Assert.Equal(2097152D, fs.Kilobytes);
            Assert.Equal(2048D, fs.Megabytes);
            Assert.Equal(2D, fs.Gigabytes);
            Assert.Equal(0.001953125D, fs.Terabytes);
            Assert.Equal(1.9073E-06D, fs.Petabytes);
            Assert.Equal(1.9E-09D, fs.Exabytes);
        }

        [Fact]
        public void FileSizeFromGigabytesTest()
        {
            var fs = FileSize.FromGigabytes(2);

            Assert.Equal(2147483648UL, fs.Bytes);
            Assert.Equal(2097152D, fs.Kilobytes);
            Assert.Equal(2048D, fs.Megabytes);
            Assert.Equal(2D, fs.Gigabytes);
            Assert.Equal(0.001953125D, fs.Terabytes);
            Assert.Equal(1.9073E-06D, fs.Petabytes);
            Assert.Equal(1.9E-09D, fs.Exabytes);
        }

        [Fact]
        public void FileSizeFromTerabytesTest()
        {
            var fs = FileSize.FromTerabytes(0.001953125D);

            Assert.Equal(2147483648UL, fs.Bytes);
            Assert.Equal(2097152D, fs.Kilobytes);
            Assert.Equal(2048D, fs.Megabytes);
            Assert.Equal(2D, fs.Gigabytes);
            Assert.Equal(0.001953125D, fs.Terabytes);
            Assert.Equal(1.9073E-06D, fs.Petabytes);
            Assert.Equal(1.9E-09D, fs.Exabytes);
        }

        [Fact]
        public void FileSizeFromPetabytesTest()
        {
            var fs = FileSize.FromPetabytes(1.9073E-06D);

            Assert.Equal(2147428892UL, fs.Bytes);
            Assert.Equal(2097098.52734375D, fs.Kilobytes);
            Assert.Equal(2047.9477806091D, fs.Megabytes);
            Assert.Equal(1.9999490045D, fs.Gigabytes);
            Assert.Equal(0.0019530752D, fs.Terabytes);
            Assert.Equal(1.9073E-06D, fs.Petabytes);
            Assert.Equal(1.9E-09D, fs.Exabytes);
        }

        [Fact]
        public void FileSizeFromExabytesTest()
        {
            var fs = FileSize.FromExabytes(1.9E-09D);

            Assert.Equal(2190550859UL, fs.Bytes);
            Assert.Equal(2139209.8232421875D, fs.Kilobytes);
            Assert.Equal(2089.0720930099D, fs.Megabytes);
            Assert.Equal(2.0401094658D, fs.Gigabytes);
            Assert.Equal(0.0019922944D, fs.Terabytes);
            Assert.Equal(1.9456E-06D, fs.Petabytes);
            Assert.Equal(1.9E-09D, fs.Exabytes);
        }

        #endregion From Tests

        #region Operator Tests

        ////[Fact]
        ////public void FileSizeOperatorNegateTest_Overflow()
        ////{
        ////    FileSize fs1 = FileSize.MinValue;
        ////    Assert.Throws<OverflowException>(() => -fs1);
        ////}

        ////[Fact]
        ////public void FileSizeOperatorNegateTest_Value()
        ////{
        ////    FileSize fs1 = FileSize.FromMegabytes(2048);
        ////    FileSize result = -fs1;

        ////    Assert.Equal(-2147483648, result.Bytes);
        ////}

        [Fact]
        public void FileSizeOperatorMinusTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromBytes(1024);

            FileSize result = fs1 - fs2;
            Assert.Equal(2147482624UL, result.Bytes);
        }

        [Fact]
        public void FileSizeOperatorPositiveTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            FileSize result = +fs1;

            Assert.Equal(2147483648, result.Bytes);
        }

        [Fact]
        public void FileSizeOperatorAddTest_MaxValues()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.MaxValue;
            FileSize result = fs1 + fs2;

            Assert.Equal(2147483647UL, result.Bytes);
        }

        [Fact]
        public void FileSizeOperatorAddTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048);
            FileSize result = fs1 + fs2;

            Assert.Equal(4294967296UL, result.Bytes);
        }

        [Fact]
        public void FileSizeOperatorNotEqualsTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048);

            Assert.False(fs1 != fs2, "Equal values compared incorrectly.");

            fs2 = FileSize.FromMegabytes(1024);
            Assert.True(fs1 != fs2, "Non equal values compared incorrectly.");
        }

        [Fact]
        public void FileSizeOperatorLessThanTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048);
            Assert.False(fs1 < fs2, "Equal values compared incorrectly.");

            fs2 = FileSize.FromMegabytes(1024);
            Assert.False(fs1 < fs2, "1 Non equal values compared incorrectly.");
            Assert.True(fs2 < fs1, "2 Non equal values compared incorrectly.");
        }

        [Fact]
        public void FileSizeOperatorLessThanEqualTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048);
            Assert.True(fs1 <= fs2, "Equal values compared incorrectly.");

            fs2 = FileSize.FromMegabytes(1024);
            Assert.False(fs1 <= fs2, "1 Non equal values compared incorrectly.");
            Assert.True(fs2 <= fs1, "2 Non equal values compared incorrectly.");
        }

        [Fact]
        public void FileSizeOperatorGreaterThanTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048);
            Assert.False(fs1 > fs2, "Equal values compared incorrectly.");

            fs2 = FileSize.FromMegabytes(1024);
            Assert.True(fs1 > fs2, "1 Non equal values compared incorrectly.");
            Assert.False(fs2 > fs1, "2 Non equal values compared incorrectly.");
        }

        [Fact]
        public void FileSizeOperatorGreaterThanEqualTest_Values()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048);
            Assert.True(fs1 >= fs2, "Equal values compared incorrectly.");

            fs2 = FileSize.FromMegabytes(1024);
            Assert.True(fs1 >= fs2, "1 Non equal values compared incorrectly.");
            Assert.False(fs2 >= fs1, "2 Non equal values compared incorrectly.");
        }

        #endregion Operator Tests

        #region Equals Tests

        [Fact]
        public void FileSizeEqualsObjectTest_Null()
        {
            var fs = FileSize.FromMegabytes(2048);
            object obj = null;

            bool result = fs.Equals(obj);
            Assert.False(result, "Null object compared incorrectly.");
        }

        [Fact]
        public void FileSizeEqualsObjectTest_Object()
        {
            var fs = FileSize.FromMegabytes(2048);
            var obj = new object();

            bool result = fs.Equals(obj);
            Assert.False(result, "Object compared incorrectly.");
        }

        [Fact]
        public void FileSizeEqualsObjectTest_TrueValue()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(2048) as object;

            bool result = fs1.Equals(fs2);
            Assert.True(result, "Object compared incorrectly.");
        }

        [Fact]
        public void FileSizeEqualsObjectTest_FalseValue()
        {
            var fs1 = FileSize.FromMegabytes(2048);
            var fs2 = FileSize.FromMegabytes(1024) as object;

            bool result = fs1.Equals(fs2);
            Assert.False(result, "Object compared incorrectly.");
        }

        #endregion Equals Tests

        #region GetHashCode Tests

        [Theory]
        [InlineData(2048, -2147483648)]
        [InlineData(1125452.548, -993475186)]
        public void FileSizeGetHashCodeTest(double input, int expectedResult)
        {
            var fs1 = FileSize.FromMegabytes(input);
            int result = fs1.GetHashCode();

            Assert.Equal(expectedResult, result);
        }

        #endregion GetHashCode Tests

        #region Negate Tests

        ////[Fact]
        ////public void FileSizeNegateTest_Overflow()
        ////{
        ////    var fs1 = FileSize.MinValue;
        ////    Assert.Throws<OverflowException>(() => fs1.Negate());
        ////}

        ////[Fact]
        ////public void FileSizeNegateTest_Value()
        ////{
        ////    var fs1 = FileSize.FromMegabytes(2048);
        ////    FileSize result = fs1.Negate();

        ////    Assert.Equal(-2147483648, result.Bytes);
        ////}

        #endregion Negate Tests

        #region ToString Tests

        [Fact]
        public void FileSizeToStringTest_Bytes()
        {
            var fs = FileSize.FromBytes(2);
            string result = fs.ToString();

            Assert.Equal("2.00 B", result);
        }

        [Fact]
        public void FileSizeToStringTest_Kilobytes()
        {
            var fs = FileSize.FromKilobytes(2.69345);
            string result = fs.ToString();

            Assert.Equal("2.69 KB", result);
        }

        [Fact]
        public void FileSizeToStringTest_Megabytes()
        {
            var fs = FileSize.FromMegabytes(2.69345);
            string result = fs.ToString();

            Assert.Equal("2.69 MB", result);
        }

        [Fact]
        public void FileSizeToStringTest_Gigabytes()
        {
            var fs = FileSize.FromGigabytes(2.69345);
            string result = fs.ToString();

            Assert.Equal("2.69 GB", result);
        }

        [Fact]
        public void FileSizeToStringTest_Terabytes()
        {
            var fs = FileSize.FromTerabytes(2.69345);
            string result = fs.ToString();

            Assert.Equal("2.69 TB", result);
        }

        [Fact]
        public void FileSizeToStringTest_Petabytes()
        {
            var fs = FileSize.FromPetabytes(2.69345);
            string result = fs.ToString();

            Assert.Equal("2.69 PB", result);
        }

        [Fact]
        public void FileSizeToStringTest_Exabytes()
        {
            var fs = FileSize.FromExabytes(2.69345);
            string result = fs.ToString();

            Assert.Equal("2.69 EB", result);
        }

        #endregion ToString Tests
    }
}