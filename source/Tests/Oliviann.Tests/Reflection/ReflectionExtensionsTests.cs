namespace Oliviann.Tests.Reflection
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using Oliviann.Reflection;
    using Oliviann.Tests.TestObjects;
    using Moq;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ReflectionExtensionsTests
    {
        #region GetInformationalVersion Tests

        /// <summary>
        /// Verifies a null assembly object returns back a null value.
        /// </summary>
        [Fact]
        public void GetInformationalVersionTest_NullAssembly()
        {
            Assembly assy = null;
            string result = assy.GetInformationalVersion();

            Assert.Null(result);
        }

        /// <summary>
        /// Verifies the current executing assembly returns back the correct
        /// information version.
        /// </summary>
        [Fact]
        public void GetInformationalVersionTest_CurrentAssembly()
        {
            Assembly assy = Assembly.GetExecutingAssembly();
            string result = assy.GetInformationalVersion();

            Assert.Equal("1.0.1.9", result);
        }

        /// <summary>
        /// Verifies if no attribute is found then a null is returned.
        /// </summary>
        [Fact]
        public void GetInformationalVersionTest_CurrentAssembly_NullAttribute()
        {
            var assyMoc = new Mock<TestAssembly>();
            assyMoc.SetupGet(a => a.CustomAttributes).Returns(new List<CustomAttributeData>());

            string result = assyMoc.Object.GetInformationalVersion();
            Assert.Null(result);
        }

        #endregion GetInformationalVersion Tests

        #region GetCurrentExecutingDirectory Tests

        /// <summary>
        /// Verifies an exception is thrown when passing a null assembly.
        /// </summary>
        [Fact]
        public void GetCurrentExecutingDirectory_NullAssembly()
        {
            Assembly assy = null;
            Assert.Throws<ArgumentNullException>(() => assy.GetCurrentExecutingDirectory());
        }

        /// <summary>
        /// Verifies the current executing assembly returns back a executing
        /// directory path greater than 50 characters.
        /// </summary>
        /// <remarks>
        /// Can't verify exact path because it may be different on different
        /// machines.
        /// </remarks>
        [Fact]
        public void GetCurrentExecutingDirectory_CurrentAssembly()
        {
            Assembly assy = Assembly.GetExecutingAssembly();
            string result = assy.GetCurrentExecutingDirectory();

            Trace.WriteLine("Path: " + result);
            Assert.True(result.Length > 50, "Current assembly path was less than 50 characters.");
        }

        #endregion GetCurrentExecutingDirectory Tests

        #region Property GetValue Tests

#if NET40

        [Fact]
        public void PropertyGetValue_NullProperty()
        {
            Assert.Throws<NullReferenceException>(() => ReflectionExtensions.GetValue(null, null));
        }

        [Fact]
        public void PropertyGetValue_NullInstance()
        {
            var moc = new GenericMocTestClass { PropString = "Hello" };

            PropertyInfo pi = moc.GetType().GetProperty("PropString");
            Assert.Throws<TargetException>(() => { object result = ReflectionExtensions.GetValue(pi, null); });
        }

        [Fact]
        public void PropertyGetValue_StringProperty()
        {
            var moc = new GenericMocTestClass { PropString = "Hello" };

            PropertyInfo pi = moc.GetType().GetProperty("PropString");
            object result = ReflectionExtensions.GetValue(pi, moc);
            var strResult = result as string;

            Assert.Equal("Hello", strResult);
        }

        [Fact]
        public void PropertyGetValue_IntProperty()
        {
            var moc = new GenericMocTestClass { PropInt = 27 };

            PropertyInfo pi = moc.GetType().GetProperty("PropInt");
            object result = ReflectionExtensions.GetValue(pi, moc);
            var intResult = (int)result;

            Assert.Equal(27, intResult);
        }

#endif

        #endregion Property GetValue Tests

        #region Property SetValue Tests

#if NET40

        [Fact]
        public void PropertySetValue_NullProperty()
        {
            Assert.Throws<NullReferenceException>(() => ReflectionExtensions.SetValue(null, null, null));
        }

        [Fact]
        public void PropertySetValue_NullInstance()
        {
            var moc = new GenericMocTestClass { PropString = "Hello" };

            PropertyInfo pi = moc.GetType().GetProperty("PropString");
            Assert.Throws<TargetException>(() => ReflectionExtensions.SetValue(pi, null, null));
        }

        [Fact]
        public void PropertySetValue_StringProperty()
        {
            var moc = new GenericMocTestClass { PropString = "Hello" };

            PropertyInfo pi = moc.GetType().GetProperty("PropString");
            ReflectionExtensions.SetValue(pi, moc, "World");
            string result = moc.PropString;

            Assert.Equal("World", result);
        }

        [Fact]
        public void PropertySetValue_IntProperty()
        {
            var moc = new GenericMocTestClass { PropInt = 27 };

            PropertyInfo pi = moc.GetType().GetProperty("PropInt");
            ReflectionExtensions.SetValue(pi, moc, -6);
            int result = moc.PropInt;

            Assert.Equal(-6, result);
        }

#endif

        #endregion Property SetValue Tests

        public class TestAssembly : Assembly
        {
            public TestAssembly()
            {
            }
        }
    }
}