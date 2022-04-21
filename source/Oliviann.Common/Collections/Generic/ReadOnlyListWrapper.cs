#if NET35 || NET40

namespace Oliviann.Collections.Generic
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by
    /// index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class ReadOnlyListWrapper<T> : List<T>, IReadOnlyList<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ReadOnlyListWrapper{T}"/> class.
        /// </summary>
        public ReadOnlyListWrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ReadOnlyListWrapper{T}"/> class that contains elements
        /// copied from the specified collection and has sufficient capacity to
        /// accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied
        /// to the new list.</param>
        public ReadOnlyListWrapper(IEnumerable<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ReadOnlyListWrapper{T}"/> class that is empty and has
        /// the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can
        /// initially store.</param>
        public ReadOnlyListWrapper(int capacity) : base(capacity < 0 ? 0 : capacity)
        {
        }

        #endregion
    }
}

#endif