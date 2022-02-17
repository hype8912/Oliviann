namespace Oliviann.Data.Common
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Oliviann.Properties;
    using Reflection;
    using SqlKata;
    using SqlKata.Compilers;

    #endregion

    /// <summary>
    /// Represents a base query string builder class.
    /// </summary>
    public class DbQueryStringBuilder<T>
    {
        #region Fields

        /// <summary>
        /// The current compiler for the query.
        /// </summary>
        private readonly Compiler compiler;

        /// <summary>
        /// Cache for the specified types reflection data.
        /// </summary>
        private readonly IEnumerable<Tuple<string, Func<T, object>, Attribute[]>> propertyData;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DbQueryStringBuilder{T}"/> class.
        /// </summary>
        public DbQueryStringBuilder(T entity, Compiler compilerType)
        {
            this.CurrentModel = entity;
            this.compiler = compilerType;
            this.propertyData = this.GetTypeCache();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parameters that should be excluded from the query
        /// compilation.
        /// </summary>
        public List<string> ExcludedParameters { get; set; } = new List<string>();

        /// <summary>
        /// Gets the collection of parameters and values retrieved from the
        /// entity.
        /// </summary>
        public IDictionary<string, object> Parameters { get; protected set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets the current entity model.
        /// </summary>
        protected T CurrentModel { get; }

        #endregion

        #region Insert Methods

        /// <summary>
        /// Builds an insert query for all the properties that aren't excluded.
        /// </summary>
        /// <returns>A SQL query string for the specified type parameters.
        /// </returns>
        public virtual string CompileInsert()
        {
            TableData table = this.GetTypeData(true);
            var paramColumns = table.Columns.ToDictionary(c => c.Name, c => c.Value);

            var query = new Query(table.Name).AsInsert(paramColumns);
            SqlResult result = this.compiler.Compile(query);
            this.Parameters = result.NamedBindings;
            return result.Sql;
        }

        #endregion

        #region Select Methods

        /// <summary>
        /// Builds a select query for all the properties that aren't excluded.
        /// </summary>
        /// <returns>
        /// A SQL query string for the specified type parameters.</returns>
        public virtual string CompileSelect()
        {
            TableData table = this.GetTypeData(false);
            var query = new Query(table.Name).Select(table.Columns.Select(c => c.Name).ToArray());
            this.Parameters.Clear();

            return this.compiler.Compile(query).Sql;
        }

        /// <summary>
        /// Builds a select query for all the properties that aren't excluded
        /// using the [Key] attributes for filtering.
        /// </summary>
        /// <returns>
        /// A SQL query string for the specified type parameters.</returns>
        public virtual string CompileSelectByKey() => this.Select(this.GetTypeData(true));

        /// <summary>
        /// Builds a select query for all the properties that aren't excluded
        /// using the specified key column name for filtering.
        /// </summary>
        /// <param name="keyColumnNames">The primary key column name(s).</param>
        /// <returns>A SQL query string for the specified type parameters
        /// </returns>
        public virtual string CompileSelectByKey(params string[] keyColumnNames)
        {
            TableData table = this.GetTypeData(true, keyColumnNames);
            return this.Select(table);
        }

        #endregion

        #region Update Methods

        /// <summary>
        /// Builds an update query for all the properties that aren't excluded
        /// using the [Key] attributes for filtering.
        /// </summary>
        /// <returns>A SQL query string for the specified type parameters.
        /// </returns>
        public virtual string CompileUpdate() => this.Update(this.GetTypeData(true));

        /// <summary>
        /// Builds an update query for all the properties that aren't excluded
        /// using the specified key column name for filtering.
        /// </summary>
        /// <param name="keyColumnNames">The primary key column name(s).</param>
        /// <returns>A SQL query string for the specified type parameters
        /// </returns>
        public virtual string CompileUpdate(params string[] keyColumnNames)
        {
            TableData table = this.GetTypeData(true, keyColumnNames);
            return this.Update(table);
        }

        #endregion

        #region Delete Methods

        /// <summary>
        /// Builds a delete query for all the properties that aren't excluded
        /// using the [Key] attributes for filtering.
        /// </summary>
        /// <returns>A SQL query string for the specified type parameters.
        /// </returns>
        public virtual string CompileDelete() => this.Delete(this.GetTypeData(true));

        /// <summary>
        /// Builds a delete query for all the properties that aren't excluded
        /// using the specified key column name for filtering only.
        /// </summary>
        /// <param name="keyColumnNames">The primary key column name(s).</param>
        /// <returns>A SQL query string for the specified type parameters.
        /// </returns>
        /// <remarks>This overrides any [Key] attributes on the class.</remarks>
        public virtual string CompileDelete(params string[] keyColumnNames)
        {
            TableData table = this.GetTypeData(true, keyColumnNames);
            return this.Delete(table);
        }

        #endregion

        #region Protected CRUD Methods

        /// <summary>
        /// Builds a select query for the specified table data.
        /// </summary>
        /// <param name="table">The table data to build the query for.</param>
        /// <returns>A SQL query string for the specified type parameters.
        /// </returns>
        protected string Select(TableData table)
        {
            // Build the where clauses for the keys.
            var query = new Query(table.Name).Select(table.Columns.Select(c => c.Name).ToArray());
            foreach (ColumnData column in table.Columns.Where(c => c.IsPrimaryKey))
            {
                query.WhereEquals(column.Name, column.Value);
            }

            SqlResult result = this.compiler.Compile(query);
            this.Parameters = result.NamedBindings;
            return result.Sql;
        }

        /// <summary>
        /// Builds an update query for the specified table data.
        /// </summary>
        /// <param name="table">The table data to build the query for.</param>
        /// <returns>A SQL query string for the specified type parameters.
        /// </returns>
        protected string Update(TableData table)
        {
            // Validate there is at least 1 key column.
            if (!table.Columns.Any(c => c.IsPrimaryKey))
            {
                throw new KeyNotFoundException(Resources.ERR_NoKeyColumnsFound);
            }

            // Build the where clauses and parameters.
            var query = new Query(table.Name);
            var paramColumns = new Dictionary<string, object>();
            foreach (ColumnData column in table.Columns)
            {
                if (column.IsPrimaryKey)
                {
                    query.WhereEquals(column.Name, column.Value);
                }
                else
                {
                    paramColumns.Add(column.Name, column.Value);
                }
            }

            SqlResult result = this.compiler.Compile(query.AsUpdate(paramColumns));
            this.Parameters = result.NamedBindings;
            return result.Sql;
        }

        /// <summary>
        /// Builds a delete query for the specified table data.
        /// </summary>
        /// <param name="table">The table data to build the query for.</param>
        /// <returns>A SQL query string for the specified type parameters.
        /// </returns>
        protected string Delete(TableData table)
        {
            // Validate there is at least 1 key column.
            if (!table.Columns.Any(c => c.IsPrimaryKey))
            {
                throw new KeyNotFoundException(Resources.ERR_NoKeyColumnsFound);
            }

            // Build the where clauses for the keys.
            var query = new Query(table.Name);
            foreach (ColumnData column in table.Columns.Where(c => c.IsPrimaryKey))
            {
                query.WhereEquals(column.Name, column.Value);
            }

            SqlResult result = this.compiler.Compile(query.AsDelete());
            this.Parameters = result.NamedBindings;
            return result.Sql;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets all the needed data for the specified type.
        /// </summary>
        /// <param name="includeValues">True to include the property values;
        /// otherwise, false.</param>
        /// <param name="keyColumnNames">Optional. The collection of column
        /// names to make primary keys. Note: The will override any [Key]
        /// attributes on the class and only use the specified keys.</param>
        /// <returns>All the available table data for the type.</returns>
        protected TableData GetTypeData(bool includeValues, string[] keyColumnNames = null)
        {
            var table = new TableData { Name = this.GetTableName(typeof(T)), Columns = this.GetFilteredParameters(includeValues) };
            if (table.Columns.Count < 1)
            {
                throw new MissingMemberException(Resources.ERR_NoPropertiesFound);
            }

            if (keyColumnNames == null || keyColumnNames.Length < 1)
            {
                return table;
            }

            table.Columns.ForEach(c => c.IsPrimaryKey = keyColumnNames.Contains(c.Name, StringComparer.OrdinalIgnoreCase));
            return table;
        }

        /// <summary>
        /// Retrieves all the parameters and the required data.
        /// </summary>
        /// <param name="includeValues">True to include the property values;
        /// otherwise, false.</param>
        /// <returns>A collection of tuples containing all the properties data.
        /// </returns>
        protected List<ColumnData> GetFilteredParameters(bool includeValues)
        {
            bool filtersAvailable = this.ExcludedParameters?.Count > 0;

            var columns = new List<ColumnData>();
            foreach (var data in this.propertyData)
            {
                // Exclude properties with the NotMappedAttribute.
                if (data.Item3.Any(a => a is NotMappedAttribute))
                {
                    continue;
                }

                // Exclude the properties that are in the excluded parameters
                // collection.
                ColumnData column = this.GetColumn(data.Item1, data.Item3);
                if (filtersAvailable &&
                    this.ExcludedParameters.Contains(column.Name, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                column.Value = includeValues ? data.Item2(this.CurrentModel) : null;
                columns.Add(column);
            }

            return columns.OrderBy(c => c.Order).ThenBy(c => c.Name).ToList();
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <param name="currentType">The current class member.</param>
        /// <returns>The name of the table.</returns>
        protected string GetTableName(MemberInfo currentType)
        {
            ADP.CheckArgumentNull(currentType, nameof(currentType));
            if (currentType.GetCustomAttributeCached(typeof(TableAttribute), false) is TableAttribute attr &&
                !attr.Name.IsNullOrWhiteSpace())
            {
                return attr.Name;
            }

            return currentType.Name;
        }

        /// <summary>
        /// Gets the column order and name for the specified property.
        /// </summary>
        /// <param name="propertyName">The current name of the property.</param>
        /// <param name="propertyAttributes">The attributes applied to the
        /// current property.</param>
        /// <returns>A column data object containing all the information about
        /// the column.</returns>
        protected ColumnData GetColumn(string propertyName, Attribute[] propertyAttributes)
        {
            ADP.CheckArgumentNullOrEmpty(propertyName, nameof(propertyName));
            ADP.CheckArgumentNull(propertyAttributes, nameof(propertyAttributes));

            var data = new ColumnData { Name = propertyName, Order = -1 };
            foreach (Attribute attribute in propertyAttributes)
            {
                if (attribute is KeyAttribute)
                {
                    data.IsPrimaryKey = true;
                }
                else if (attribute is ColumnAttribute column && !column.Name.IsNullOrWhiteSpace())
                {
                    data.Name = column.Name;
                    data.Order = column.Order;
                }
            }

            return data;
        }

        /// <summary>
        /// Gets all the needed reflection data for the properties of the
        /// specified type.
        /// </summary>
        /// <returns>A collection of tuples containing the reflection data.
        /// </returns>
        /// <remarks>Retrieving the attributes is the most is the most expensive
        /// part followed by getting the value.</remarks>
        private IReadOnlyCollection<Tuple<string, Func<T, object>, Attribute[]>> GetTypeCache()
        {
            var items = new List<Tuple<string, Func<T, object>, Attribute[]>>();
            ParameterExpression instanceParam = Expression.Parameter(typeof(T));

            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var getter = Expression.Lambda<Func<T, object>>(
                    Expression.Convert(
                        Expression.Call(instanceParam, property.GetGetMethod(true)),
                        typeof(object)), instanceParam).Compile();

                items.Add(new Tuple<string, Func<T, object>, Attribute[]>(property.Name, getter, property.GetCustomAttributesCached(false)));
            }

            return items;
        }

        #endregion
    }
}