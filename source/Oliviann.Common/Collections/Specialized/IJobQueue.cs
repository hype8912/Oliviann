namespace Oliviann.Collections.Specialized
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using Oliviann.Collections.Generic;
    using Oliviann.ComponentModel;

    #endregion

    /// <summary>
    /// Represents an interface for implementing a generic Job Queue.
    /// </summary>
    public interface IJobQueue<T> : IReadOnlyCollection<Job<T>>
    {
        #region Events

        /// <summary>
        /// Occurs anytime a log event is invoked. Argument 1 is the LogLevel integer value.
        /// Argument 2 is the message.
        /// </summary>
        event EventHandler<EventArgs<int, string>> Logger;

        #endregion

        #region Proeprties

        /// <summary>
        /// Get the currently configured job options.
        /// </summary>
        JobQueueOptions Options { get; }

        /// <summary>
        ///  Gets the number of jobs in the waiting queue.
        /// </summary>
        int WaitingCount { get; }

        /// <summary>
        /// Gets the number of jobs currently in the working queue.
        /// </summary>
        int WorkingCount { get; }

        /// <summary>
        /// Gets or sets the delegate to executed when the job is ready to be
        /// processed.
        /// </summary>
        Action<Job<T>> WorkerAction { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new job to the waiting queue.
        /// </summary>
        /// <param name="newJob">The job object to be added to the queue for
        /// processing.</param>
        /// <returns>The order number of the job in the queue.</returns>
        int Enqueue(Job<T> newJob);

        /// <summary>
        /// Determines what order the current job is in the waiting queue at
        /// that specific moment.
        /// </summary>
        /// <param name="jobId">The unique job id.</param>
        /// <returns>A value indicating the order in the waiting queue if found,
        /// otherwise, -1.</returns>
        int GetJobOrder(Guid jobId);

        #endregion
    }
}