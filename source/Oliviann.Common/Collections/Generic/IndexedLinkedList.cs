namespace Oliviann.Collections.Generic
{
    #region Usings

    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents an indexed doubly linked list.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the indexed linked
    /// list.</typeparam>
    public class IndexedLinkedList<T>
    {
        #region Fields

        /// <summary>
        /// The internal linked list for the nodes.
        /// </summary>
        private readonly LinkedList<T> data;

        /// <summary>
        /// The internal index dictionary for the linked nodes.
        /// </summary>
        private readonly Dictionary<T, LinkedListNode<T>> index;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedLinkedList{T}"/>
        /// class.
        /// </summary>
        public IndexedLinkedList()
        {
            this.data = new LinkedList<T>();
            this.index = new Dictionary<T, LinkedListNode<T>>();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="IndexedLinkedList{T}"/>.
        /// </summary>
        public int Count
        {
            get { return this.data.Count; }
        }

        /// <summary>
        /// Gets the first node of the <see cref="IndexedLinkedList{T}"/>.
        /// </summary>
        public T First
        {
            get { return this.data.First.Value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds an item to the <see cref="IndexedLinkedList{T}"/>.
        /// </summary>
        /// <param name="item">The value to add to the
        /// <see cref="IndexedLinkedList{T}"/>.</param>
        public void Add(T item)
        {
            this.index[item] = this.data.AddLast(item);
        }

        /// <summary>
        /// Removes all nodes from the <see cref="IndexedLinkedList{T}"/>.
        /// </summary>
        public void Clear()
        {
            this.data.Clear();
            this.index.Clear();
        }

        /// <summary>
        /// Removes the first occurrence of the specified value from the
        /// <see cref="IndexedLinkedList{T}"/>.
        /// </summary>
        /// <param name="value">The value to remove from the
        /// <see cref="IndexedLinkedList{T}"/>.</param>
        /// <remarks>Possibly change this to not throw an exception but just
        /// return if a null value is passed in.</remarks>
        public void Remove(T value)
        {
            if (!this.index.TryGetValue(value, out LinkedListNode<T> node))
            {
                return;
            }

            this.data.Remove(value);
            this.index.Remove(value);
        }

        /// <summary>
        /// Removes the node at the start of the
        /// <see cref="IndexedLinkedList{T}"/>.
        /// </summary>
        public void RemoveFirst()
        {
            this.index.Remove(this.data.First.Value);
            this.data.RemoveFirst();
        }

        #endregion Methods
    }
}