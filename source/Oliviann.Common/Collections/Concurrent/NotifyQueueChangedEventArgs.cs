namespace Oliviann.Collections.Concurrent
{
    /// <summary>
    /// Represents a data for a notify queue changed event.
    /// </summary>
    /// <typeparam name="T">The type of item contained in the event queue.
    /// </typeparam>
    public class NotifyQueueChangedEventArgs<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NotifyQueueChangedEventArgs{T}" /> class.
        /// </summary>
        /// <param name="action">The action that caused the event.</param>
        public NotifyQueueChangedEventArgs(NotifyQueueChangedAction action)
        {
            this.Action = action;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NotifyQueueChangedEventArgs{T}" /> class.
        /// </summary>
        /// <param name="action">The action that caused the event.</param>
        /// <param name="changedItem">The item that is affected by the change.
        /// </param>
        public NotifyQueueChangedEventArgs(NotifyQueueChangedAction action, T changedItem)
        {
            this.Action = action;
            this.ChangedItem = changedItem;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the action that caused the event.
        /// </summary>
        /// <value>
        /// The
        /// <see cref="T:Oliviann.Collections.Concurrent.NotifyQueueChangedAction"/>
        /// value that describes the action that caused the event.
        /// </value>
        public NotifyQueueChangedAction Action { get; }

        /// <summary>
        /// Gets the item involved in the change.
        /// </summary>
        /// <value>
        /// The item involved in the change.
        /// </value>
        public T ChangedItem { get; }

        #endregion Properties
    }
}