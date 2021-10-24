namespace Oliviann.Linq.Expressions
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a class for comparing objects using simple lambda
    /// expressions.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.This type parameter
    /// is contravariant. That is, you can use either the type you specified or
    /// any type that is less derived.</typeparam>
    /// <remarks>
    /// See
    /// <a href="http://brendan.enrick.com/post/linq-your-collections-with-iequalitycomparer-and-lambda-expressions.aspx">
    /// link</a> for more info.
    /// </remarks>
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        #region Fields

        /// <summary>
        /// Place holder for the lambda equality comparer.
        /// </summary>
        private readonly Func<T, T, bool> lambdaEqualityComparer;

        /// <summary>
        /// Place holder for the lambda object hasher.
        /// </summary>
        private readonly Func<T, int> lambdaHasher;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T}"/>
        /// class.
        /// </summary>
        /// <param name="lambdaComparer">The lambda equality comparer.</param>
        public LambdaComparer(Func<T, T, bool> lambdaComparer) : this(lambdaComparer, o => 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaComparer{T}"/>
        /// class.
        /// </summary>
        /// <param name="lambdaComparer">The lambda equality comparer.</param>
        /// <param name="lambdaHash">The lambda hash.</param>
        public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
        {
            ADP.CheckArgumentNull(lambdaComparer, nameof(lambdaComparer));
            ADP.CheckArgumentNull(lambdaHash, nameof(lambdaHash));

            this.lambdaEqualityComparer = lambdaComparer;
            this.lambdaHasher = lambdaHash;
        }

        #endregion Constructor/Destructor

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>
        /// <c>True</c> if the specified objects are equal; otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool Equals(T x, T y) => this.lambdaEqualityComparer(x, y);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing
        /// algorithms and data structures like a hash table.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The type of object
        /// is a reference type and object is null.</exception>
        public int GetHashCode(T obj) => this.lambdaHasher(obj);
    }
}