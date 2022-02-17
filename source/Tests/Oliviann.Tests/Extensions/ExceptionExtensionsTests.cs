namespace Oliviann.Tests.Extensions
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ExceptionExtensionsTests
    {
        [Fact]
        public void AggregateMessageText_NullException()
        {
            Exception ex = null;
            string result = ex.AggregateMessage();

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void AggregateMessageText_EmptyException()
        {
            var ex = new Exception(string.Empty);
            string result = ex.AggregateMessage();

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void AggregateMessageText_ExceptionMessage()
        {
            var ex = new Exception("This is an exception.");
            string result = ex.AggregateMessage();

            Assert.Equal("This is an exception.", result);
        }

        [Fact]
        public void AggregateMessageText_ChildException()
        {
            var ex1 = new Exception("This is an inner exception.");
            var ex2 = new Exception("This is a parent exception.", ex1);
            string result = ex2.AggregateMessage();

            Assert.Equal("This is a parent exception. This is an inner exception.", result);
        }

        [Fact]
        public void AggregateMessageText_ChildException_NullSeparator()
        {
            var ex1 = new Exception("This is an inner exception.");
            var ex2 = new Exception("This is a parent exception.", ex1);
            string result = ex2.AggregateMessage(null);

            Assert.Equal("This is a parent exception.This is an inner exception.", result);
        }

        [Fact]
        public void AggregateMessageText_ChildException_PipeSeparator()
        {
            var ex1 = new Exception("This is an inner exception.");
            var ex2 = new Exception("This is a parent exception.", ex1);
            string result = ex2.AggregateMessage("||");

            Assert.Equal("This is a parent exception.||This is an inner exception.", result);
        }

        [Fact]
        public void AddNewException_NullBoth()
        {
            AggregateException ae = null;
            Assert.Null(ae.AddNewException(null));
        }

        [Fact]
        public void AddNewException_NullException()
        {
            var ae = new AggregateException("Test Message");
            AggregateException result = ae.AddNewException(null);

            Assert.Equal("Test Message", result.Message);
        }

        [Fact]
        public void AddNewException_NullAggregate()
        {
            AggregateException ae = null;
            var ex = new Exception("Test Message");
            AggregateException result = ae.AddNewException(ex);

            Assert.NotNull(result);
            Assert.Single(result.InnerExceptions);
        }

        [Fact]
        public void AddNewException_MultipleExceptions()
        {
            var lst = new List<Exception>
                {
                    new Exception("Test Message 1"),
                    new Exception("Test Message 2")
                };

            var ae = new AggregateException(lst);
            var ex = new Exception("Test Message 3");
            AggregateException result = ae.AddNewException(ex);

            Assert.NotNull(result);
            Assert.Equal(3, result.InnerExceptions.Count);
        }
    }
}