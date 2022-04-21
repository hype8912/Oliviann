namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using Oliviann.Security.Cryptography;

    #endregion

    /// <summary>
    /// Represents a node in a BinaryTree{T}.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the binary tree.
    /// </typeparam>
    public sealed class BinaryTreeNode<T> : IEquatable<BinaryTreeNode<T>> where T : IComparable<T>, IComparable
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}"/>
        /// class.
        /// </summary>
        public BinaryTreeNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}"/>
        /// class.
        /// </summary>
        /// <param name="parent">The parent node of the current node.</param>
        public BinaryTreeNode(BinaryTreeNode<T> parent) : this(parent, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTreeNode{T}"/>
        /// class, containing the specified value.
        /// </summary>
        /// <param name="parent">The parent node of the current node.</param>
        /// <param name="value">The value to contain in the current node.
        /// </param>
        public BinaryTreeNode(BinaryTreeNode<T> parent, T value)
        {
            this.Parent = parent;
            this.Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Determines if the current instance has any children.
        /// </summary>
        public bool HasChildren => this.Left != null || this.Right != null;

        /// <summary>
        /// Gets or sets the left node in the binary tree.
        /// </summary>
        public BinaryTreeNode<T> Left { get; set; }

        /// <summary>
        /// Gets or sets the right node in the binary tree.
        /// </summary>
        public BinaryTreeNode<T> Right { get; set; }

        /// <summary>
        /// Gets or sets the current nodes value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets the parent node in the binary tree.
        /// </summary>
        public BinaryTreeNode<T> Parent { get; }

        #endregion

        #region Operators

        /// <summary>Indicates whether the specified node values are equal to
        /// each other. </summary>
        /// <param name="node1">The first node to compare.</param>
        /// <param name="node2">The other node to compare to the first node.
        /// </param>
        /// <returns>True if the nodes are equal; otherwise, false.</returns>
        public static bool operator ==(BinaryTreeNode<T> node1, BinaryTreeNode<T> node2)
        {
            if (ReferenceEquals(node1, node2))
            {
                return true;
            }

            if (node1 == null && node2 == null)
            {
                return true;
            }

            if ((node1 != null && node2 == null) || (node1 == null && node2 != null))
            {
                return false;
            }

            return node1.Value.Equals(node2.Value);
        }

        /// <summary>Indicates whether the specified node values are not equal
        /// to each other. </summary>
        /// <param name="node1">The first node to compare.</param>
        /// <param name="node2">The other node to compare to the first node.
        /// </param>
        /// <returns>True if the nodes are not equal; otherwise, false.
        /// </returns>
        public static bool operator !=(BinaryTreeNode<T> node1, BinaryTreeNode<T> node2) => !(node1 == node2);

        #endregion

        #region Methods

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is BinaryTreeNode<T> other && this.Equals(other);

        /// <inheritdoc />
        public bool Equals(BinaryTreeNode<T> other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(this.Value);

        #endregion
    }
}