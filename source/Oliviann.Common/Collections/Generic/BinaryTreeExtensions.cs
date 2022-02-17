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
        /// Gets the max depth of the tree starting at the specified node.
        /// </summary>
        /// <param name="tree">The binary tree instance.</param>
        /// <returns>An integer that represents the max levels of the tree.
        /// </returns>
        public static int GetDepth<T>(this BinaryTree<T> tree) where T : IComparable => tree.GetDepth(tree?.Root);

        /// <summary>
        /// Finds the first node that matches the specified value.
        /// </summary>
        /// <param name="tree">The bianry tree instance.</param>
        /// <param name="value">The value to be matched.</param>
        /// <returns>The matching node if found, otherwise; null.</returns>
        public static BinaryTreeNode<T> Find<T>(this BinaryTree<T> tree, T value) where T : IComparable => tree?.Find(tree.Root, value);
    }
}