namespace Oliviann.Collections.Concurrent
{
    /// <summary>
    /// Represents the method that handles the queue changed event.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the queue.
    /// </typeparam>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Information about the event.</param>
    public delegate void NotifyQueueChangedEventHandler<T>(object sender, NotifyQueueChangedEventArgs<T> e);
}