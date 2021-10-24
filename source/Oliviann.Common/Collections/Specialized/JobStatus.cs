namespace Oliviann.Collections.Specialized
{
    /// <summary>
    /// Defines the status of a job.
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// New job and hasn't been added to the queue.
        /// </summary>
        New,

        /// <summary>
        /// In the queue waiting to be scheduled.
        /// </summary>
        InQueue,

        /// <summary>
        /// Scheduled in the work queue to begin processing.
        /// </summary>
        Waiting,

        /// <summary>
        /// Currently being processed.
        /// </summary>
        Running,

        /// <summary>
        /// All actions of the job have been completed.
        /// </summary>
        Complete,

        /// <summary>
        /// An error occurred with the job.
        /// </summary>
        Error
    }
}