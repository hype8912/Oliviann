namespace Oliviann.Linq.Dynamic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion Usings

    /// <summary>
    /// Represents a collection of dynamic linq extension methods.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a
        /// key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The key property to sort by.</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are
        /// sorted according to a key.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property) =>
            ApplyOrder(source, property, "OrderBy");

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a
        /// key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The key property to sort by.</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are
        /// sorted in descending order according to a key.</returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property) =>
            ApplyOrder(source, property, "OrderByDescending");

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in
        /// ascending order according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The key property to sort by.</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are
        /// sorted according to a key.</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property) =>
            ApplyOrder(source, property, "ThenBy");

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in
        /// descending order, according to a key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The key property to sort by.</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are
        /// sorted in descending order according to a key.</returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property) =>
            ApplyOrder(source, property, "ThenByDescending");

        /// <summary>
        /// Converts an <see cref="IQueryable"/> collection to a
        /// <see cref="IList"/> generic list.
        /// </summary>
        /// <param name="query">The <see cref="IQueryable"/> object to be
        /// converted.</param>
        /// <returns>A generic list containing the same data as the specified
        /// queryable collection.</returns>
        public static IList ToList(this IQueryable query)
        {
            ADP.CheckArgumentNull(query, nameof(query));
            return typeof(List<>).MakeGenericType(query.ElementType).CreateInstance<IList>(query);
        }

        /// <summary>
        /// Applies the sort method to the specified collection according to a
        /// key.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="property">The key property to sort by.</param>
        /// <param name="methodName">Sort name of the method.</param>
        /// <returns>An <see cref="IOrderedQueryable{T}"/> whose elements are
        /// sorted according to a key.</returns>
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            ADP.CheckArgumentNull(source, nameof(source));
            ADP.CheckArgumentNull(property, nameof(property));

            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            string[] props = property.Split('.');
            foreach (string prop in props)
            {
                // Use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                {
                    continue;
                }

                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}