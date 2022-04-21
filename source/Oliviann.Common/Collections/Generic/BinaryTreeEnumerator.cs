namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    internal class BinaryTreeEnumerator<T> : IEnumerator<T>, IEnumerator, IDisposable where T : IComparable<T>, IComparable
    {
        #region Fields

        private BinaryTreeNode<T> current;
        private BinaryTree<T> tree;
        internal Queue<BinaryTreeNode<T>> traverseQueue;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BinaryTreeEnumerator{T}"/> class.
        /// </summary>
        /// <param name="tree">The binary tree instance.</param>
        public BinaryTreeEnumerator(BinaryTree<T> tree)
        {
            this.tree = tree;
            this.traverseQueue = new Queue<BinaryTreeNode<T>>();
        }

        #endregion

        #region Properties

        public T Current => this.Current;

        object IEnumerator.Current => this.Current;

        #endregion

        #region Methods

        public void Dispose()
        {
            this.traverseQueue?.Clear();
            this.current = null;
            this.tree = null;
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            if (this.traverseQueue.Count > 0)
            {
                current = traverseQueue.Dequeue();
            }
            else
            {
                current = null;
            }

            return this.current != null;
        }

        public void Reset() => this.current = null;

        private void TouchNode(BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            this.traverseQueue.Enqueue(node);
            this.TouchNode(node.Left);
            this.TouchNode(node.Right);
        }

        #endregion
    }
}