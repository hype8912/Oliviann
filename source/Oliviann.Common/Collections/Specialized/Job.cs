namespace Oliviann.Collections.Specialized
{
    #region Usings

    using System;
    using Oliviann.ComponentModel;

    #endregion

    /// <summary>
    /// Represents a job for the <see cref="ObservableJobQueue{T}"/>.
    /// </summary>
    public class Job<T> : NotifyPropertyChange
    {
        #region Fields

        /// <summary>
        /// The backing field for the job status.
        /// </summary>
        private JobStatus _status = JobStatus.New;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Job{T}"/> class.
        /// </summary>
        /// <param name="payload">The payload needed for processing the current
        /// job.</param>
        public Job(T payload)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique job identifier.
        /// </summary>
        /// <value>The unique job identifier.</value>
        public Guid JobId { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the payload for processing the job.
        /// </summary>
        /// <value>The payload data for processing the job.</value>
        public T Payload { get; set; }

        /// <summary>
        /// Gets or sets the job status. Will trigger a property change event
        /// on change.
        /// </summary>
        /// <value>The current status.</value>
        public JobStatus Status
        {
            get => this._status;
            internal set => this.SetProperty(ref this._status, value);
        }

        #endregion
    }
}