namespace Oliviann.Collections.Concurrent
{
    /// <summary>
    /// Notifies listeners of dynamic changes, such as when items get enqueued
    /// and dequeued or the whole collection is refreshed.
    /// </summary>
    /// <typeparam name="T">The type of the item contained in the queue.
    /// </typeparam>
    public interface INotifyQueueChanged<T>
    {
        /// <summary>
        /// Occurs when the queue changes.
        /// </summary>
        event NotifyQueueChangedEventHandler<T> QueueChanged;
    }
}