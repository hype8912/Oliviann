namespace Oliviann.Collections.Concurrent
{
    #region Usings

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a thread-safe observable first in-first out (FIFO)
    /// collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the queue.
    /// </typeparam>
    [Serializable]
    public sealed class ObservableConcurrentQueue<T> : ConcurrentQueue<T>, INotifyQueueChanged<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ObservableConcurrentQueue{T}"/> class.
        /// </summary>
        public ObservableConcurrentQueue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ObservableConcurrentQueue{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to
        /// the new
        /// <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.
        /// </param>
        public ObservableConcurrentQueue(IEnumerable<T> collection) : base(collection)
        {
        }

        #endregion Constructor/Destructor

        #region Events

        /// <summary>
        /// Occurs when the queue changes.
        /// </summary>
        public event NotifyQueueChangedEventHandler<T> QueueChanged;

        #endregion Events

        #region Methods

        /// <summary>
        /// Adds an object to the end of the queue.
        /// </summary>
        /// <param name="item">The object to add to the end of the queue. The
        /// value can be a null reference (Nothing in Visual Basic) for
        /// reference types.</param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            this.QueueChanged?.Invoke(this, new NotifyQueueChangedEventArgs<T>(NotifyQueueChangedAction.Enqueue, item));
        }

        /// <summary>
        /// Attempts to remove and return the object at the beginning of the
        /// queue.
        /// </summary>
        /// <param name="result">When this method returns, if the operation was
        /// successful, <paramref name="result" /> contains the object removed.
        /// If no object was available to be removed, the value is unspecified.
        /// </param>
        /// <returns>
        /// True if an element was removed and returned from the beginning of
        /// the queue successfully; otherwise, false.
        /// </returns>
        public new bool TryDequeue(out T result)
        {
            if (!base.TryDequeue(out result))
            {
                return false;
            }

            this.QueueChanged?.Invoke(this, new NotifyQueueChangedEventArgs<T>(NotifyQueueChangedAction.Dequeue, result));

            if (this.IsEmpty)
            {
                this.QueueChanged?.Invoke(this, new NotifyQueueChangedEventArgs<T>(NotifyQueueChangedAction.Empty));
            }

            return true;
        }

        #endregion Methods
    }
}