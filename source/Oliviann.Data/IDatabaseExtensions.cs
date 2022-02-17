namespace Oliviann.Data
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="IDatabase"/> objects.
    /// </summary>
    public static class IDatabaseExtensions
    {
        /// <summary>
        /// Filters a collection of <see cref="IDatabase"/> objects by the
        /// specified <paramref name="databaseName">database name</paramref>.
        /// </summary>
        /// <param name="items">The collection of database to be filtered.
        /// </param>
        /// <param name="databaseName">Name of the database to match.</param>
        /// <returns>
        /// A single <see cref="IDatabase"/> object if a match is found;
        /// otherwise, the default value.
        /// </returns>
        public static IDatabase FilterByName(this IEnumerable<IDatabase> items, string databaseName) =>
            items?.FirstOrDefault(d => d.Name.EqualsOrdinalIgnoreCase(databaseName));
    }
}