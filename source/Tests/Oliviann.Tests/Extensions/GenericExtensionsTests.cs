namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Common.TestObjects.Xml;
    using Oliviann.IO;
    using TestObjects;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class GenericExtensionsTests
    {
        #region AsLazy Tests

        [Fact]
        public void AsLazyTest_NullFunction()
        {
            Func<string> function = null;
            Assert.Throws<ArgumentNullException>(() => function.AsLazy());
        }

        [Fact]
        public void AsLazyTest_ValueFunction()
        {
            Func<string> function = () => "Tacos";
            Lazy<string> result = function.AsLazy();

            Assert.NotNull(result);
            Assert.Equal("Tacos", result.Value);
        }

        [Fact]
        public void AsLazyFromValueTest_NullValue()
        {
            string value = null;
            Lazy<string> result = value.AsLazyFromValue();

            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public void AsLazyFromValueTest_ValidValue()
        {
            string value = "Tacos";
            Lazy<string> result = value.AsLazyFromValue();

            Assert.NotNull(result);
            Assert.Equal("Tacos", result.Value);
        }

        #endregion

        #region CloneT Tests

        [Fact]
        public void CloneTTest_Null()
        {
            string nullString = null;
            Assert.Throws<NullReferenceException>(() => nullString.CloneT());
        }

        [Fact]
        public void CloneTTest_Object()
        {
            var orig = new MocCloneableClass
                {
                    PropInt = 99,
                    PropTestClass =
                        {
                            PropInt = 100,
                            PropString = "Test"
                        }
                };

            var copy = orig.CloneT();
            copy.PropInt = 25;
            copy.PropTestClass.PropInt = 50;
            copy.PropTestClass.PropString = "Hello";

            Assert.Equal(99, orig.PropInt);
            Assert.Equal(25, copy.PropInt);

            Assert.Equal(50, orig.PropTestClass.PropInt);
            Assert.Equal("Hello", orig.PropTestClass.PropString);
        }

        #endregion CloneT Tests

        #region Copy Tests

        [Fact]
        public void CopyTest_Null()
        {
            string nullString = null;
            string cpy = nullString.Copy();
        }

        [Fact]
        public void CopyTest_NotSerializableClass()
        {
            var sect = new Section("Name");
            Assert.Throws<ArgumentException>(() => sect.Copy());
        }

        [Fact]
        public void CopyTest_Object()
        {
            var orig = new MocCloneableClass
                {
                    PropInt = 99,
                    PropTestClass =
                        {
                            PropInt = 100,
                            PropString = "Test"
                        }
                };

            var copy = orig.Copy();
            copy.PropInt = 25;
            copy.PropTestClass.PropInt = 50;
            copy.PropTestClass.PropString = "Hello";

            Assert.Equal(99, orig.PropInt);
            Assert.Equal(25, copy.PropInt);

            Assert.Equal(100, orig.PropTestClass.PropInt);
            Assert.Equal("Test", orig.PropTestClass.PropString);
            Assert.Equal(50, copy.PropTestClass.PropInt);
            Assert.Equal("Hello", copy.PropTestClass.PropString);
        }

        #endregion Copy Tests

        #region GetSafeHashCode Tests

        [Fact]
        public void GetSafeHashCodeTest_Null()
        {
            string nullString = null;
            int result = nullString.GetSafeHashCode();
            Assert.Equal(0, result);
        }

        [Fact]
        public void GetSafeHashCodeTest_String()
        {
            const string TestString = "Test";
            const int ExpectedResult1 = -354185577;
            int result = TestString.GetSafeHashCode();

            if (result == ExpectedResult1)
            {
                Assert.Equal(result, ExpectedResult1);
                return;
            }

            // This is to match the hash code returned on the build box. This method
            // may return different results based on .NET Framework versions.
            const int ExpectedResult2 = -871204762;
            Assert.Equal(result, ExpectedResult2);
        }

        #endregion GetSafeHashCode Tests

        #region GetValueOrDefault Tests

        /// <summary>
        /// Verifies the default result is returned when the nullable doesn't
        /// have a value.
        /// </summary>
        [Fact]
        public void GetValueOrDefaultTest_Nullable_NullSource()
        {
            int? source = null;
            Func<int?, string> act = i => i.Value.ToString();
            string result = source.GetValueOrDefault(act, "Hello");

            Assert.Equal("Hello", result);
        }

        /// <summary>
        /// Verifies the source result is returned when the nullable has a
        /// value.
        /// </summary>
        [Fact]
        public void GetValueOrDefaultTest_Nullable_ValueSource()
        {
            int? source = 17;
            Func<int?, string> act = i => i.Value.ToString();
            string result = source.GetValueOrDefault(act, "Hello");

            Assert.Equal("17", result);
        }

        /// <summary>
        /// Verifies the default result is returned for a null object.
        /// </summary>
        [Fact]
        public void GetValueOrDefaultTest_Ref_NullSource()
        {
            string source = null;
            Func<string, int> act = s => s.ToInt32(99);
            int result = source.GetValueOrDefault(act, 241);

            Assert.Equal(241, result);
        }

        /// <summary>
        /// Verifies the source result is returned when the source is not null.
        /// </summary>
        [Fact]
        public void GetValueOrDefaultTest_Ref_ValueSource()
        {
            string source = "17";
            Func<string, int> act = s => s.ToInt32(99);
            int result = source.GetValueOrDefault(act, 241);

            Assert.Equal(17, result);
        }

        #endregion GetValueOrDefault Tests

        #region IsDefault Tests

        /// <summary>
        /// Verifies a null object returns a true value.
        /// </summary>
        [Fact]
        public void IsDefaultTest_NullValue()
        {
            object val = null;
            bool result = val.IsDefault();

            Assert.True(result);
        }

        /// <summary>
        /// Verifies a non-null string returns a false value.
        /// </summary>
        [Fact]
        public void IsDefaultTest_NonNullValue()
        {
            string val = "Hello World!";
            bool result = val.IsDefault();

            Assert.False(result);
        }

        /// <summary>
        /// Verifies a default struct returns a true value.
        /// </summary>
        [Fact]
        public void IsDefaultTest_Struct()
        {
            var val = new KeyValuePair<string, object>();
            bool result = val.IsDefault();

            Assert.True(result);
        }

        #endregion IsDefault Tests

        #region PutList Tests

        [Theory]
        [InlineData(null, 1)]
        [InlineData("Test", 1)]
        public void PutListTest_ValidValues(string input, int expectedCount)
        {
            List<string> result = input.PutList();

            Assert.Equal(expectedCount, result.Count);
            Assert.Equal(input, result[0]);
        }

        [Fact]
        public void PutListTest_Object()
        {
            var bk = new Book { Author = "Me" };
            List<Book> result = bk.PutList();

            Assert.Single(result);
            Assert.Equal(bk.Author, result[0].Author);
        }

        #endregion PutList Tests

        #region With Tests

        [Fact]
        public void ActionWith_NullObject()
        {
            Action<GenericMocTestClass> act = c => c.PropBool = true;

            GenericMocTestClass nullObject = null;
            Assert.Throws<NullReferenceException>(() => nullObject.With(act));
        }

        [Fact]
        public void ActionWith_NullAction()
        {
            var mocObject = new GenericMocTestClass();
            Assert.Throws<NullReferenceException>(() => mocObject.With(null));
        }

        [Fact]
        public void ActionWith_ExecuteAction()
        {
            Action<GenericMocTestClass> act = c =>
                {
                    c.PropBool = true;
                    c.PropInt = 100;
                    c.PropString = "Test";
                };

            var mocObject = new GenericMocTestClass();
            mocObject.With(act);

            Assert.True(mocObject.PropBool, "Action bool property wasn't set correctly.");
            Assert.Equal(100, mocObject.PropInt);
            Assert.Equal("Test", mocObject.PropString);
        }

        #endregion With Tests
    }
}