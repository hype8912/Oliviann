namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a generic binary tree object.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the binary tree.
    /// </typeparam>
    public class BinaryTree<T> : IEnumerable<T>, IEnumerable where T : IComparable<T>, IComparable

    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTree{T}"/> class.
        /// </summary>
        public BinaryTree()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryTree{T}"/> class.
        /// </summary>
        /// <param name="rootNode">The node to be set as the root node.</param>
        public BinaryTree(BinaryTreeNode<T> rootNode)
        {
            this.Root = rootNode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the root tree node.
        /// </summary>
        public BinaryTreeNode<T> Root { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the left node of the specified parent tree node.
        /// </summary>
        /// <param name="parent">The parent tree node to be updated.</param>
        /// <param name="node">The node to be assigned.</param>
        public void InsertLeft(BinaryTreeNode<T> parent, BinaryTreeNode<T> node)
        {
            if (parent != null)
            {
                parent.Left = node;
            }
        }

        /// <summary>
        /// Sets the right node of the specified parent tree node.
        /// </summary>
        /// <param name="parent">The parent tree node to be updated.</param>
        /// <param name="node">The node to be assigned.</param>
        public void InsertRight(BinaryTreeNode<T> parent, BinaryTreeNode<T> node)
        {
            if (parent != null)
            {
                parent.Right = node;
            }
        }

        /// <summary>
        /// Finds the first node that matches the specified value.
        /// </summary>
        /// <param name="parentNode">The node to start searching from.</param>
        /// <param name="searchValue">The value to be matched.</param>
        /// <returns>The matching node if found, otherwise; null.</returns>
        public BinaryTreeNode<T> Find(BinaryTreeNode<T> parentNode, T searchValue)
        {
            if (parentNode == null)
            {
                return null;
            }

            if (parentNode.Value.Equals(searchValue))
            {
                return parentNode;
            }

            BinaryTreeNode<T> node = this.Find(parentNode.Left, searchValue);
            if (node != null)
            {
                return node;
            }

            node = this.Find(parentNode.Right, searchValue);
            return node;
        }

        //public int GetLevel(BinaryTreeNode<T> node, int level = 1)
        //{
        //    if (node == null)
        //    {
        //        return -1;
        //    }

        //    BinaryTreeNode<T> leftNode = null;
        //    BinaryTreeNode<T> rightNode = null;
        //}

        /// <summary>
        /// Gets the max height of the tree starting at the specified node.
        /// </summary>
        /// <param name="node">The node to start from.</param>
        /// <returns>An integer that represents the max height of the tree.
        /// </returns>
        public int GetHeight(BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                return 0;
            }

            int lHeight = GetHeight(node.Left);
            int rHeight = GetHeight(node.Right);

            if (lHeight > rHeight)
            {
                return lHeight + 1;
            }

            return rHeight + 1;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator() => new BinaryTreeEnumerator<T>(this);

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion
    }
}