namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Hosting;
    using System.Windows.Forms;

    using Oliviann.IO;

    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ObjectExtensionsTests
    {
        #region Reflection

        public static IEnumerable<object[]> GetPropertyValueExceptionValues =>
        new List<object[]>
                {
                    new object[] { null, "PropString", typeof(NullReferenceException) },
                    new object[] { new GenericMocTestClass { PropInt = 100, PropString = "Test" }, null, typeof(ArgumentNullException) },
                    new object[] { new GenericMocTestClass { PropInt = 100, PropString = "Test" }, "TacoBell", typeof(MissingMemberException) },
                    new object[] { new GenericMocTestClass { PropInt = 100, PropString = "Test" }, "propstring", typeof(MissingMemberException) },
                };

        [Theory]
        [MemberData(nameof(GetPropertyValueExceptionValues))]
        public void GetPropertyValueTest_Exceptions(GenericMocTestClass input, string propertyName, Type expectedException)
        {
            Assert.Throws(expectedException, () => input.GetPropertyValue(propertyName));
        }

        [Fact]
        public void GetPropertyValueTest_Objects()
        {
            var moc = new GenericMocTestClass { PropInt = 100, PropString = "Test" };

            object result1 = moc.GetPropertyValue("PropString");
            object result2 = moc.GetPropertyValue("PropInt");
            object result3 = moc.GetPropertyValue("SetOnlyProp");

            Assert.Equal("Test", (string)result1);
            Assert.Equal(100, (int)result2);
            Assert.Null(result3);
        }

        public static IEnumerable<object[]> GetPropertyValueTExceptionValues =>
            new List<object[]>
                {
                    new object[] { null, "PropString", typeof(NullReferenceException) },
                    new object[] { new GenericMocTestClass { PropInt = 100, PropString = "Test" }, null, typeof(ArgumentNullException) },
                    new object[] { new GenericMocTestClass { PropInt = 100, PropString = "Test" }, "TacoBell", typeof(MissingMemberException) },
                    new object[] { new GenericMocTestClass { PropInt = 100, PropString = "Test" }, "propstring", typeof(MissingMemberException) },
                };

        [Theory]
        [MemberData(nameof(GetPropertyValueTExceptionValues))]
        public void GetPropertyValueTTest_Exception(GenericMocTestClass input, string propertyName, Type expectedException)
        {
            Assert.Throws(expectedException, () => input.GetPropertyValue<string>(propertyName));
        }

        [Fact]
        public void GetPropertyValueTTest_Objects()
        {
            var moc = new GenericMocTestClass { PropInt = 100, PropString = "Test" };

            var result1 = moc.GetPropertyValue<string>("PropString");
            var result2 = moc.GetPropertyValue<int>("PropInt");
            var result3 = moc.GetPropertyValue<object>("SetOnlyProp");

            Assert.Equal("Test", result1);
            Assert.Equal(100, result2);
            Assert.Null(result3);
        }

        #endregion Reflection

        #region ItemText Tests

        [Fact]
        public void ItemTextTest_NullItem()
        {
            ListBox box = null;
            string result = box.ItemText(string.Empty);
            Assert.Equal(result, string.Empty);
        }

        [Fact]
        public void ItemTextTest_NullMember()
        {
            var box = new ListBox();
            box.Items.AddRange(new object[] { "Item1", "Item2", "Item3", "Item4", "Item5" });
            string result = box.ItemText(null);

            Assert.Equal(result, string.Empty);
        }

        [Fact]
        public void ItemTextTest_Items()
        {
            var items = new List<GenericMocTestClass>
                {
                    new GenericMocTestClass { PropInt = 10, PropString = "Test10" },
                    new GenericMocTestClass { PropInt = 20, PropString = "Test20" },
                    new GenericMocTestClass { PropInt = 30, PropString = "Test30" },
                    new GenericMocTestClass { PropInt = 40, PropString = "Test40" },
                    new GenericMocTestClass { PropInt = 50, PropString = "Test50" },
                };

            var box = new ListBox { DisplayMember = "PropString", ValueMember = "PropInt" };
            box.Items.AddRange(items.ToArray());
            var iz = box.Items;
            var oi = box.Items[0];

            string result = box.Items[2].ItemText(box.DisplayMember);
            Assert.Equal("Test30", result);
        }

        #endregion ItemText Tests

        #region Validations

        public static IEnumerable<object[]> IsNotNullTestValues =>
            new List<object[]>
            {
                new[] { TestValues.NullObject, false },
                new object[] { TestValues.NullString, false },
                new[] { new object(), true },
                new object[] { "Test", true }
            };

        [Theory]
        [MemberData(nameof(IsNotNullTestValues))]
        public void IsNotNullTest(object input, bool expectedResult)
        {
            Assert.Equal(expectedResult, input.IsNotNull());
        }

        public static IEnumerable<object[]> IsNullTestValues =>
            new List<object[]>
                {
                    new[] { TestValues.NullObject, true },
                    new object[] { TestValues.NullString, true },
                    new[] { new object(), false },
                    new object[] { "Test", false }
                };

        [Theory]
        [MemberData(nameof(IsNullTestValues))]
        public void IsNullTest(object input, bool expectedResult)
        {
            Assert.Equal(expectedResult, input.IsNull());
        }

        [Fact]
        public void IsNumericTest_NullString()
        {
            Assert.False(TestValues.NullString.IsNumeric(), "Null string was found to be a numeric value.");
        }

        [Theory]
        [InlineData(16, true)]
        [InlineData("16", true)]
        [InlineData("16.0", true)]
        [InlineData("16.26", true)]
        [InlineData(26.99, true)]
        [InlineData("-61", true)]
        [InlineData(-81.2, true)]
        [InlineData(TestValues.NullObject, false)]
        [InlineData("", false)]
        [InlineData("A21", false)]
        [InlineData("0x21", false)]
        [InlineData("&()3", false)]
        public void IsNumericTest_Values(object value, bool expectedResult)
        {
            Assert.Equal(expectedResult, value.IsNumeric());
        }

        #endregion Validations

        #region Converts

        #region ToByteArray Tests

        [Fact]
        public void ToByteArrayTest_Null()
        {
            Assert.Null(TestValues.NullObject.ToStream().ToArray(true));
        }

        [Fact]
        public void ToByteArrayTest_Object()
        {
            string str = "Test Text";
            string result = str.ToStream().ToArray(true).ToHexString();
            Assert.Equal("01000FFFFFFFF10000000610009546573742054657874B", result);
        }

        #endregion ToByteArray Tests

        #region ToDateTimeDefault Tests

        [Fact]
        public void ToDateTimeDefaultTest_Null()
        {
            DateTime value = DateTime.UtcNow;
            DateTime result = TestValues.NullString.ToDateTime(value);
            Assert.Equal(result, value);
        }

        [Fact]
        public void ToDateTimeDefaultTest_Value()
        {
            DateTime value1 = DateTime.UtcNow;
            DateTime result1 = value1.ToDateTime(new DateTime(2013, 01, 01));
            Assert.Equal(result1.Date,  value1.Date);

            DateTime result2 = "asdbk234821t*^T".ToDateTime(value1);
            Assert.Equal(result2.Date,  value1.Date);
        }

        #endregion ToDateTimeDefault Tests

        #region ToDateTimeNullable Tests

        [Fact]
        public void ToDateTimeNullableTest_Null()
        {
            Assert.Null(TestValues.NullString.ToDateTime());
            Assert.Null(TestValues.NullObject.ToDateTime());
        }

        [Fact]
        public void ToDateTimeNullableTest_True()
        {
            var temp = new DateTime(2013, 02, 27);
            DateTime? result = temp.ToShortDateString().ToDateTime();

            Assert.NotNull(result);
            Assert.Equal(result.Value, temp);
        }

        [Fact]
        public void ToDateTimeNullableTest_Junk()
        {
            DateTime? result = "^90erfgh3&".ToDateTime();
            Assert.False(result.HasValue);
        }

        #endregion ToDateTimeNullable Tests

        #region ToDateTimeExact Tests

        [Fact]
        public void ToDateTimeExactTest_NullValue()
        {
            DateTime? result = TestValues.NullObject.ToDateTimeExact(null);
            Assert.Null(result);
        }

        [Fact]
        public void ToDateTimeExactTest_NullFormat()
        {
            var value = DateTime.UtcNow;
            DateTime? result = value.ToString().ToDateTimeExact(null);
            Assert.Null(result);
        }

        [Fact]
        public void ToDateTimeExactTest_Junk()
        {
            DateTime? result = "^90erfgh3&".ToDateTimeExact("g");
            Assert.False(result.HasValue);
        }

        [Fact]
        public void ToDateTimeExactTest_Values()
        {
            string value = "5/01/2009 8:30 AM";
            DateTime? result = value.ToDateTimeExact("g");

            Assert.True(result.HasValue);
            Assert.Equal("5/1/2009", result.Value.Date.ToShortDateString());
        }

        #endregion ToDateTimeExact Tests

        #region ToSignedValue Tests

        public static IEnumerable<object[]> DecimalData => TestValues.DecimalValues.Select(r => new[] { r.Item1, r.Item3 });

        [Theory]
        [MemberData(nameof(DecimalData))]
        public void ToDecimalTest_Values(object value, decimal expectedValue)
        {
            decimal result = value.ToDecimal();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableDecimalData => TestValues.DecimalValues.Select(r => new[] { r.Item1, r.Item2, r.Item4 });

        [Theory]
        [MemberData(nameof(NullableDecimalData))]
        public void ToDecimalNullableTest_Values(object value, bool expectedHasValue, decimal? expectedValue)
        {
            decimal? result = value.ToDecimalNullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        ////[MemberData(nameof(DecimalData))]
        [InlineData("21", 21.0)]
        [InlineData("26.99", 26.99)]
        [InlineData("A&*(", 0)]
        [InlineData("-21.42", -21.42)]
        [InlineData(null, 0)]
        public void ToDoubleTest_Values(object value, double expectedValue)
        {
            double result = value.ToDouble();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableDoubleData => TestValues.DecimalValues.Select(r => new[] { r.Item1, r.Item2, (double?)r.Item4 });

        [Theory]
        [MemberData(nameof(NullableDoubleData))]
        public void ToDoubleNullableTest_Values(object value, bool expectedHasValue, double? expectedValue)
        {
            double? result = value.ToDoubleNullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> Int16Data => TestValues.Int16Values.Select(r => new[] { r.Item1, r.Item3 });

        [Theory]
        [MemberData(nameof(Int16Data))]
        public void ToInt16Test_Values(object value, short expectedValue)
        {
            short result = value.ToInt16();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableInt16Data => TestValues.Int16Values.Select(r => new[] { r.Item1, r.Item2, r.Item4 });

        [Theory]
        [MemberData(nameof(NullableInt16Data))]
        public void ToInt16NullableTest_Values(object value, bool expectedHasValue, short? expectedValue)
        {
            short? result = value.ToInt16Nullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> Int32Data => TestValues.Int32Values.Select(r => new[] { r.Item1, r.Item3 });

        [Theory]
        [MemberData(nameof(Int16Data))]
        [MemberData(nameof(Int32Data))]
        public void ToInt32Test_Values(object value, int expectedValue)
        {
            int result = value.ToInt32();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableInt32Data => TestValues.Int32Values.Select(r => new[] { r.Item1, r.Item2, r.Item4 });

        [Theory]
        [MemberData(nameof(NullableInt16Data))]
        [MemberData(nameof(NullableInt32Data))]
        public void ToInt32NullableTest_Values(object value, bool expectedHasValue, int? expectedValue)
        {
            int? result = value.ToInt32Nullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> Int64Data => TestValues.Int64Values.Select(r => new[] { r.Item1, r.Item3 });

        [Theory]
        [MemberData(nameof(Int16Data))]
        [MemberData(nameof(Int32Data))]
        [MemberData(nameof(Int64Data))]
        public void ToInt64Test_Values(object value, long expectedValue)
        {
            long result = value.ToInt64();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableInt64Data => TestValues.Int64Values.Select(r => new[] { r.Item1, r.Item2, r.Item4 });

        [Theory]
        [MemberData(nameof(NullableInt16Data))]
        [MemberData(nameof(NullableInt32Data))]
        [MemberData(nameof(NullableInt64Data))]
        public void ToInt64NullableTest_Values(object value, bool expectedHasValue, long? expectedValue)
        {
            long? result = value.ToInt64Nullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        ////[MemberData(nameof(DecimalData))]
        [InlineData("21", 21.0)]
        [InlineData("26.99", 26.99)]
        [InlineData("A&*(", 0)]
        [InlineData("-21.42", -21.42)]
        [InlineData(null, 0)]
        public void ToSingleTest_Values(object value, float expectedValue)
        {
            float result = value.ToSingle();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableSingleData => TestValues.DecimalValues.Select(r => new[] { r.Item1, r.Item2, (float?)r.Item4 });

        [Theory]
        [MemberData(nameof(NullableSingleData))]
        public void ToSingleNullableTest_Values(object value, bool expectedHasValue, float? expectedValue)
        {
            float? result = value.ToSingleNullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        #endregion ToSignedValue Tests

        #region ToString Tests

        /// <summary>
        /// This method is to prove that the .NET Framework throws an error
        /// when you call ToString() on a null object. In .NET 1.1 this didn't
        /// happen so this is what drove the creation of the ToStringSafe()
        /// method.
        /// </summary>
        [Fact]
        public void ToStringNotSafeTest_Null()
        {
            Assert.Throws<NullReferenceException>(() => TestValues.NullObject.ToString());
        }

        public static IEnumerable<object[]> ToStringSafeObjectStrings =>
            new List<object[]>
                {
                    new[] { TestValues.NullObject, string.Empty },
                    new object[] { "21", "21" },
                    new object[] { "26.99", "26.99" },
                    new object[] { "A&*(", "A&*(" },
                    new object[] { "-21.42", "-21.42" },

                    new[] { new object(), "System.Object" },
                    new object[] { new EventArgs(), "System.EventArgs" },
                    new object[] { new ApplicationActivator(), "System.Runtime.Hosting.ApplicationActivator" }
                };

        [Theory]
        [MemberData(nameof(ToStringSafeObjectStrings))]
        public void ToStringSafeTest_Values(object input, string expectedResult)
        {
            string result = input.ToStringSafe();
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> ToStringTrimObjectStrings =>
            new List<object[]>
            {
                new[] { TestValues.NullObject, string.Empty },
                new object[] { "21 ", "21" },
                new object[] { " 26.99", "26.99" },
                new object[] { "   A&*(   ", "A&*(" },
                new object[] { " -21.42", "-21.42" },

                new[] { new object(), "System.Object" },
                new object[] { new EventArgs(), "System.EventArgs" },
                new object[] { new ApplicationActivator(), "System.Runtime.Hosting.ApplicationActivator" }
            };

        [Theory]
        [MemberData(nameof(ToStringTrimObjectStrings))]
        public void ToStringTrimTest_Values(object input, string expectedResult)
        {
            string result = input.ToStringTrim();
            Assert.Equal(expectedResult, result);
        }

        #endregion ToString Tests

        #region ToUnsignedValue Tests

        public static IEnumerable<object[]> UInt16Data => TestValues.UInt16Values.Select(r => new[] { r.Item1, r.Item3 });

        [Theory]
        [MemberData(nameof(UInt16Data))]
        public void ToUInt16Test_Values(object value, ushort expectedValue)
        {
            ushort result = value.ToUInt16();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableUInt16Data => TestValues.UInt16Values.Select(r => new[] { r.Item1, r.Item2, r.Item4 });

        [Theory]
        [MemberData(nameof(NullableUInt16Data))]
        public void ToUInt16NullableTest_Values(object value, bool expectedHasValue, ushort? expectedValue)
        {
            ushort? result = value.ToUInt16Nullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> UInt32Data => TestValues.UInt32Values.Select(r => new[] { r.Item1, r.Item3 });

        [Theory]
        [MemberData(nameof(UInt16Data))]
        [MemberData(nameof(UInt32Data))]
        public void ToUInt32Test_Values(object value, uint expectedValue)
        {
            uint result = value.ToUInt32();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableUInt32Data => TestValues.UInt32Values.Select(r => new[] { r.Item1, r.Item2, r.Item4 });

        [Theory]
        [MemberData(nameof(NullableUInt16Data))]
        [MemberData(nameof(NullableUInt32Data))]
        public void ToUInt32NullableTest_Values(object value, bool expectedHasValue, uint? expectedValue)
        {
            uint? result = value.ToUInt32Nullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> UInt64Data => TestValues.UInt64Values.Select(r => new[] { r.Item1, r.Item3 });

        [Theory]
        [MemberData(nameof(UInt16Data))]
        [MemberData(nameof(UInt32Data))]
        [MemberData(nameof(UInt64Data))]
        public void ToUInt64Test_Values(object value, ulong expectedValue)
        {
            ulong result = value.ToUInt64();
            Assert.Equal(result, expectedValue);
        }

        public static IEnumerable<object[]> NullableUInt64Data => TestValues.UInt64Values.Select(r => new[] { r.Item1, r.Item2, r.Item4 });

        [Theory]
        [MemberData(nameof(NullableUInt16Data))]
        [MemberData(nameof(NullableUInt32Data))]
        [MemberData(nameof(NullableUInt64Data))]
        public void ToUInt64NullableTest_Values(object value, bool expectedHasValue, ulong? expectedValue)
        {
            ulong? result = value.ToUInt64Nullable();
            Assert.Equal(expectedHasValue, result.HasValue);
            Assert.Equal(result, expectedValue);
        }

        #endregion ToUnsignedValue Tests

        #region TryCast Tests

        [Fact]
        public void TryCastTest_NullObject()
        {
            object text = null;
            bool result = text.TryCast(out string value);

            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void TryCastTest_ValidObject()
        {
            object text = "Test";
            bool result = text.TryCast(out string value);

            Assert.True(result);
            Assert.IsType<string>(value);
            Assert.Equal("Test", value);
        }

        #endregion

        #endregion Converts
    }
}