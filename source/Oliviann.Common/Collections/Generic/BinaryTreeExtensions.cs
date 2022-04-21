namespace Oliviann.Collections.Generic
{
#region Usings

    using System;

    #endregion

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// BinaryTree{T} objects.
    /// </summary>
    public static class BinaryTreeExtensions
    {
        /// <summary>
        /// Gets the max height of the tree.
        /// </summary>
        /// <param name="tree">The binary tree instance.</param>
        /// <returns>An integer that represents the max levels of the tree.
        /// </returns>
        public static int GetHeight<T>(this BinaryTree<T> tree) where T : IComparable<T>, IComparable => tree.GetHeight(tree?.Root);

        /// <summary>
        /// Finds the first node that matches the specified value.
        /// </summary>
        /// <param name="tree">The bianry tree instance.</param>
        /// <param name="value">The value to be matched.</param>
        /// <returns>The matching node if found, otherwise; null.</returns>
        public static BinaryTreeNode<T> Find<T>(this BinaryTree<T> tree, T value) where T : IComparable<T>, IComparable => tree?.Find(tree.Root, value);

        /// <summary>
        /// Gets the first node with the specified value.
        /// </summary>
        /// <param name="tree">The bianry tree instance.</param>
        /// <param name="value">The value to be matched.</param>
        /// <param name="node">The matching node if found, otherwise; null.
        /// </param>
        /// <returns>True, if the spcified value was found; otherwise, false.
        /// </returns>
        public static bool TryFind<T>(this BinaryTree<T> tree, T value, out BinaryTreeNode<T> node) where T : IComparable<T>, IComparable
        {
            node = tree?.Find(tree.Root, value);
            return node != null;
        }
    }
}