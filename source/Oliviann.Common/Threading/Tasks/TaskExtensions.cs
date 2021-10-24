#if !NET35 && !NET40

namespace Oliviann.Threading.Tasks
{
#region Usings

    using System.Threading.Tasks;

#endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="Task"/> objects.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Waits for the task to complete and unwraps any exceptions.
        /// </summary>
        /// <param name="task">The task to wait on.</param>
        public static void WaitAndUnwrapException(this Task task)
        {
            ADP.CheckArgumentNull(task, nameof(task));
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Waits for the task to complete and unwraps any exceptions.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the task.
        /// </typeparam>
        /// <param name="task">The task to wait on.</param>
        /// <returns>The result of the completed task.</returns>
        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
        {
            ADP.CheckArgumentNull(task, nameof(task));
            return task.GetAwaiter().GetResult();
        }
    }
}

#endif