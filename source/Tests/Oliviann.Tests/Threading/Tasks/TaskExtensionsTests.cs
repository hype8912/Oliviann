#if !NET40

namespace Oliviann.Tests.Threading.Tasks
{
    #region Usings

    using System;
    using System.Threading.Tasks;
    using Oliviann.Threading.Tasks;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class TaskExtensionsTests
    {
        #region Non-Generic Tests

        /// <summary>
        /// Verifies the main thread isn't blocked when the task is completed.
        /// </summary>
        [Fact]
        public void WaitAndUnwrapExceptionTest_Completed_DoesNotBlock()
        {
            CompletedTask().WaitAndUnwrapException();
        }

        /// <summary>
        /// Verifies a faulted task throws the exception.
        /// </summary>
        [Fact]
        public void WaitAndUnwrapExceptionTest_Faulted_UnwrapsException()
        {
            Task tsk = Task.Run(() => { throw new NotImplementedException(); });
            Assert.ThrowsAny<NotImplementedException>(() => tsk.WaitAndUnwrapException());
        }

        /// <summary>
        /// Verifies the main thread isn't blocked when the task is completed.
        /// </summary>
        [Fact]
        public void WaitAndUnwrapExceptionTTest_Completed_DoesNotBlock()
        {
            Task.FromResult(99).WaitAndUnwrapException();
        }

        /// <summary>
        /// Verifies a faulted task throws the exception.
        /// </summary>
        [Fact]
        public void WaitAndUnwrapExceptionTTest_Faulted_UnwrapsException()
        {
            Task<int> tsk = Task.Run((Func<int>)(() => { throw new NotImplementedException(); }));
            Assert.Throws<NotImplementedException>(() => tsk.WaitAndUnwrapException());
        }

        #endregion Non-Generic Tests

        #region Helper Methods

        private static Task CompletedTask()
        {
#if NET45
            return Task.FromResult(true);
#else
            return Task.CompletedTask;
#endif
        }

        #endregion Helper Methods
    }
}

#endif