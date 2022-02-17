namespace Oliviann.Tests.Collections.Concurrent
{
    #region Usings

    using System.Collections.Generic;
    using Oliviann.Collections.Concurrent;
    using Oliviann.Common.TestObjects.Xml;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class ObservableConcurrentQueueTests
    {
        #region Fields

        private readonly Book bk1 = new Book { Author = "NavPress", Title = "How to find the one", Year = 1996 };
        private readonly Book bk2 = new Book { Author = "Somebody", Id = 2, Title = "Another book title", Year = 2014 };

        #endregion Fields

        /// <summary>
        /// Verifies you can pre-populate a queue with a collection.
        /// </summary>
        [Fact]
        public void ConstructorTest_AddCollection()
        {
            var books = new List<Book> { this.bk1, this.bk2 };
            var queue = new ObservableConcurrentQueue<Book>(books);

            Assert.Equal(2, queue.Count);
        }

        /// <summary>
        /// Verifies you can enqueue objects into the queue without subscribing
        /// to the events.
        /// </summary>
        [Fact]
        public void EnqueueTest_NoSubscription()

        {
            var queue = new ObservableConcurrentQueue<Book>();
            queue.Enqueue(this.bk1);

            Assert.Single(queue);
        }

        /// <summary>
        /// Verifies you can enqueue objects into the queue and the correct
        /// events notifications are returned.
        /// </summary>
        [Fact]
        public void EnqueueTest_WithSubscription()
        {
            var queue = new ObservableConcurrentQueue<Book>();
            queue.QueueChanged += (sender, args) =>
                {
                    Assert.Equal(NotifyQueueChangedAction.Enqueue, args.Action);
                    Assert.Equal(this.bk1.Title, args.ChangedItem.Title);
                };

            queue.Enqueue(this.bk1);
        }

        /// <summary>
        /// Verifies you can dequeue objects from the queue when subscribing
        /// to the events and the correct events are returned.
        /// </summary>
        [Fact]
        public void TryDequeueTest_MultipleItemsWithSubscription()
        {
            var queue = new ObservableConcurrentQueue<Book>();
            queue.Enqueue(this.bk1);
            queue.Enqueue(this.bk2);

            queue.QueueChanged += (sender, args) =>
                {
                    Assert.Equal(NotifyQueueChangedAction.Dequeue, args.Action);
                    Assert.Equal(this.bk1.Title, args.ChangedItem.Title);
                };

            bool result = queue.TryDequeue(out _);
            Assert.True(result);
        }

        /// <summary>
        /// Verifies you can dequeue objects from the queue when subscribing
        /// to the events and the correct events are returned.
        /// </summary>
        [Fact]
        public void TryDequeueTest_SingleItemWithSubscription()
        {
            var queue = new ObservableConcurrentQueue<Book>();
            queue.Enqueue(this.bk1);

            int count = 0;
            queue.QueueChanged += (sender, args) =>
                {
                    if (count < 1)
                    {
                        Assert.Equal(NotifyQueueChangedAction.Dequeue, args.Action);
                        Assert.Equal(this.bk1.Title, args.ChangedItem.Title);
                        count += 1;
                    }
                    else
                    {
                        Assert.Equal(NotifyQueueChangedAction.Empty, args.Action);
                        Assert.Null(args.ChangedItem);
                    }
                };

            bool result = queue.TryDequeue(out _);
            Assert.True(result);
        }

        /// <summary>
        /// Verifies you can dequeue objects from the queue without subscribing
        /// to the events.
        /// </summary>
        [Fact]
        public void TryDequeueTest_WithoutSubscription()
        {
            var queue = new ObservableConcurrentQueue<Book>();
            queue.Enqueue(this.bk1);
            Assert.Single(queue);

            bool result = queue.TryDequeue(out Book resultBk);
            Assert.True(result);
            Assert.Empty(queue);
            Assert.Equal(this.bk1.Title, resultBk.Title);
        }

        /// <summary>
        /// Verifies you no error is returned if you try to dequeue an item from
        /// an empty queue.
        /// </summary>
        [Fact]
        public void TryDequeueTest_EmptyQueue()
        {
            var queue = new ObservableConcurrentQueue<Book>();
            Assert.Empty(queue);

            bool result = queue.TryDequeue(out Book resultBk);
            Assert.False(result);
            Assert.Null(resultBk);
        }
    }
}