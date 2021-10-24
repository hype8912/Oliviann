namespace Oliviann.Collections.Specialized
{
    using System;

    /// <summary>
    /// Represents a set of configurable options for the job queue.
    /// </summary>
    public class JobQueueOptions
    {
        /// <summary>
        /// Backing field for max number of concurrent jobs.
        /// </summary>
        private int _maxConcurrentJobs = Environment.ProcessorCount;

        #region Properties

        /// <summary>
        /// Gets or sets the max number of jobs that can be processed at one
        /// time.
        /// </summary>
        public int MaxConcurrentJobs
        {
            get => _maxConcurrentJobs;
            set => _maxConcurrentJobs = value < 1 ? 1 : value;
        }

        #endregion
    }
}