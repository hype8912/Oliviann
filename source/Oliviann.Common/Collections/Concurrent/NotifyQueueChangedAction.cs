namespace Oliviann.Collections.Concurrent
{
    /// <summary>
    /// Describes the action that caused a queue change event.
    /// </summary>
    public enum NotifyQueueChangedAction
    {
        /// <summary>
        /// One or more items were enqueued to the queue.
        /// </summary>
        Enqueue,

        /// <summary>
        /// One or more items were dequeued to the queue.
        /// </summary>
        Dequeue,

        /// <summary>
        /// The content of the queue is empty.
        /// </summary>
        Empty
    }
}