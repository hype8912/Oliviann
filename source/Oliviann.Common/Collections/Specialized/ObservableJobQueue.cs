namespace Oliviann.Collections.Specialized
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Oliviann.Collections.Concurrent;
    using Oliviann.ComponentModel;

    #endregion

    /// <summary>
    /// Represents a generic job queue with options and status for processing
    /// jobs.
    /// </summary>
    public sealed class ObservableJobQueue<T> : IJobQueue<T>
    {
        #region Fields

        /// <summary>
        /// The queue for holding the waiting jobs.
        /// </summary>
        private readonly ObservableConcurrentQueue<Job<T>> waitingJobQueue = new ObservableConcurrentQueue<Job<T>>();

        /// <summary>
        /// The queue for holding the jobs being processed.
        /// </summary>
        private readonly ConcurrentDictionary<Guid, Job<T>> workingJobQueue = new ConcurrentDictionary<Guid, Job<T>>();

        /// <summary>
        /// Determines if the working queue is already being populated.
        /// </summary>
        private volatile bool isPopulatingWorkingJobQueue;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ObservableJobQueue{T}"/>class.
        /// </summary>
        public ObservableJobQueue() : this(new JobQueueOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ObservableJobQueue{T}"/>class.
        /// </summary>
        /// <param name="options">Unique options for the job queue.</param>
        public ObservableJobQueue(JobQueueOptions options)
        {
            this.Options = options ?? new JobQueueOptions();

            this.waitingJobQueue.QueueChanged += this.OnWaitingJobQueueChanged;
            this.OnWorkingJobAdded += this.OnWorkingJobAdded_Run;
            this.OnWorkingJobRemoved += (sender, e) => this.PopulateWorkingQueue();
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event EventHandler<EventArgs<int, string>> Logger;

        /// <summary>
        /// Occurs when a job is added to the working job queue.
        /// </summary>
        private event EventHandler<EventArgs<Job<T>>> OnWorkingJobAdded;

        /// <summary>
        /// Occurs when a job is removed from the working job queue.
        /// </summary>
        private event EventHandler<EventArgs<Job<T>>> OnWorkingJobRemoved;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the total number of jobs waiting and being processed.
        /// </summary>
        public int Count => this.waitingJobQueue.Count + this.workingJobQueue.Count;

        /// <inheritdoc />
        public JobQueueOptions Options { get; }

        /// <inheritdoc />
        public int WaitingCount => this.waitingJobQueue.Count;

        /// <inheritdoc />
        public Action<Job<T>> WorkerAction { get; set; }

        /// <inheritdoc />
        public int WorkingCount => this.workingJobQueue.Count;

        #endregion

        #region Methods

        /// <inheritdoc />
        public int Enqueue(Job<T> newJob)
        {
            ADP.CheckArgumentNull(newJob, nameof(newJob));
            newJob.PropertyChange += this.OnJobStatusChanged;

            this.waitingJobQueue.Enqueue(newJob);
            return this.waitingJobQueue.Count;
        }

        /// <inheritdoc />
        public int GetJobOrder(Guid jobId)
        {
            int count = 1;
            foreach (Job<T> job in this.waitingJobQueue)
            {
                if (job.JobId == jobId)
                {
                    return count;
                }

                count += 1;
            }

            return -1;
        }

        /// <inheritdoc />
        public IEnumerator<Job<T>> GetEnumerator() => this.waitingJobQueue.Union(this.workingJobQueue.Values).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region Helper Methods

        /// <summary>
        /// Processes the job in the working queue.
        /// </summary>
        /// <param name="sender">The sender of the job.</param>
        /// <param name="e">The job that was added to the working queue.</param>
        private void OnWorkingJobAdded_Run(object sender, EventArgs<Job<T>> e)
        {
            try
            {
                e.Value1.Status = JobStatus.Running;
                this.WorkerAction?.Invoke(e.Value1);
                e.Value1.Status = JobStatus.Complete;
            }
            catch (Exception ex)
            {
                e.Value1.Status = JobStatus.Error;
                this.LogMessage(4, $"An error occurred while processing. Id:{e.Value1.JobId} Error:{ex.Message}");
            }
            finally
            {
                // Try to remove the job from the running queue so we can
                // populate it with more jobs.
                if (this.workingJobQueue.TryRemove(e.Value1.JobId, out _))
                {
                    this.OnWorkingJobRemoved?.InvokeAsync(this, null);
                }
            }
        }

        /// <summary>
        /// Handles anytime a job is added or removed from the waiting queue.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The queue changed event arguments.</param>
        private void OnWaitingJobQueueChanged(object sender, NotifyQueueChangedEventArgs<Job<T>> e)
        {
            switch (e.Action)
            {
                case NotifyQueueChangedAction.Enqueue:

                    // Log that the new job was added to the queue and set the
                    // job status to in queue.
                    this.LogMessage(1, $"Job has been queued int the waiting queue: {e.ChangedItem.JobId}");
                    e.ChangedItem.Status = JobStatus.InQueue;

                    // Load the working collection up until it's full.
                    Task.Factory.StartNew(this.PopulateWorkingQueue);
                    break;

                case NotifyQueueChangedAction.Dequeue:
                    this.LogMessage(2, $"Job has been dequeued from the waiting queue: {e.ChangedItem.JobId}");
                    break;
            }
        }

        /// <summary>
        /// Populates the working queue with jobs from the waiting queue until
        /// the working queue has reached it's max value.
        /// </summary>
        private void PopulateWorkingQueue()
        {
            // This is a simple check to exit so the threads aren't just piling
            // up waiting on the lock to release. A few threads might get
            // through but the lock will prevent them from overloading the
            // working job queue.
            if (this.isPopulatingWorkingJobQueue)
            {
                return;
            }

            lock (this.workingJobQueue)
            {
                // Double check to prevent the possibility of a Lock Evasion.
                if (this.isPopulatingWorkingJobQueue)
                {
                    return;
                }

                this.isPopulatingWorkingJobQueue = true;

                // Execute a loop to fill up the working queue from the waiting
                // queue until the working queue reaches the maximum allowable
                // jobs or the new job queue becomes empty.
                while (this.waitingJobQueue.Count > 0 && this.workingJobQueue.Count < this.Options.MaxConcurrentJobs)
                {
                    // Try to retrieve a job from the waiting queue.
                    if (!this.waitingJobQueue.TryDequeue(out Job<T> currentJob))
                    {
                        // For some reason there was an error with dequeuing a job
                        // so we continue the loop.
                        continue;
                    }

                    // Try to take the job we just removed from the waiting queue
                    // and add it to the working job queue.
                    if (this.workingJobQueue.TryAdd(currentJob.JobId, currentJob))
                    {
                        this.LogMessage(2, $"Job is waiting to be processed: {currentJob.JobId}");
                        currentJob.Status = JobStatus.Waiting;
                        this.OnWorkingJobAdded?.InvokeAsync(this, new EventArgs<Job<T>>(currentJob));
                    }
                    else
                    {
                        // We couldn't add the job to the working job queue for
                        // some reason so add it back to the waiting queue so it can
                        // be attempted again. I understand we are adding it back to
                        // the end of the waiting queue.
                        this.waitingJobQueue.Enqueue(currentJob);
                    }
                }

                this.isPopulatingWorkingJobQueue = false;
            }
        }

        /// <summary>
        /// Anytime the job changes status we invoke the logger to notify there
        /// was a change in status.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The changed event arguments.</param>
        private void OnJobStatusChanged(object sender, PropertyChangeEventArgs e)
        {
            if (sender is Job<T> currentJob)
            {
                // Log a message that the job changed.
                this.LogMessage(2, $"Job changed status. Id:{currentJob.JobId} Old:{e.OldValue} New:{e.NewValue}");
            }
        }

        /// <summary>
        /// Invokes the log event if subscribed to.
        /// </summary>
        /// <param name="level">The LogLevel value.</param>
        /// <param name="message">The message to be logged to the consumer.</param>
        private void LogMessage(int level, string message)
        {
            this.Logger?.InvokeAsync(this, new EventArgs<int, string>(level, message));
        }

        #endregion
    }
}