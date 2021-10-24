#if NET35 || NET40

namespace Oliviann.Collections.ObjectModel
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Oliviann.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a base class for a generic collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ReadOnlyCollectionWrapper<T> : Collection<T>, IReadOnlyCollection<T>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ReadOnlyCollectionWrapper{T}"/> class.
        /// </summary>
        public ReadOnlyCollectionWrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ReadOnlyCollectionWrapper{T}"/> class as a wrapper for
        /// the specified list.
        /// </summary>
        /// <param name="list">The collection whose elements are copied to the
        /// new list.</param>
        public ReadOnlyCollectionWrapper(IList<T> list) : base(list)
        {
        }

        #endregion
    }
}

#endif