namespace Oliviann.Data
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Text;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a class for all exceptions thrown on behalf of the data
    /// source.
    /// </summary>
    [Serializable]
    public class DbDataException : DbException
    {
        #region Fields

        /// <summary>
        /// Place holder for the list of errors.
        /// </summary>
        private readonly IList<DbError> _errors;

        #endregion Fields

        #region Contsructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataException"/>
        /// class with the specified error message and a collection of database
        /// error objects.
        /// </summary>
        /// <param name="message">The error message output.</param>
        /// <param name="errors">A collection of database error objects.</param>
        internal DbDataException(string message, IEnumerable<DbError> errors) : base(message)
        {
            this._errors = new List<DbError>();
            this.HResult = -2146232009;

            errors?.ForEach(err => this._errors.Add(err));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataException"/>
        /// class.
        /// </summary>
        /// <param name="previous">The previous database exception reference.
        /// </param>
        /// <param name="inner">The inner exception reference.</param>
        /// <exception cref="NullReferenceException">Thrown when previous is
        /// null.</exception>
        internal DbDataException(DbDataException previous, Exception inner) : base(previous.Message, inner)
        {
            this.HResult = previous.ErrorCode;
            this._errors = previous.Errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataException"/>
        /// class with the specified error message and a reference to the inner
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message string.</param>
        /// <param name="inner">The inner exception reference.</param>
        internal DbDataException(string message, Exception inner) : base(message, inner)
        {
            this._errors = new List<DbError>();
            this.HResult = -2146232009;
        }

        #endregion Contsructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the collection of errors that have occurred.
        /// </summary>
        public IList<DbError> Errors
        {
            get => this._errors;
        }

        /// <summary>
        /// Gets or sets the name of the application or the object that causes
        /// the error.
        /// </summary>
        /// <returns>The name of the application or the object that causes the
        /// error.</returns>
        /// <exception cref="T:System.ArgumentException">The object must be a
        /// runtime <see cref="N:System.Reflection"/> object.</exception>
        public override string Source
        {
            get => this._errors.Count < 1 ? string.Empty : this._errors[0].Source;
        }

        #endregion Properties

        /// <summary>
        /// Creates a new database exception containing the list of specified
        /// errors.
        /// </summary>
        /// <param name="errors">The list errors that occurred.</param>
        /// <returns>A single database exception containing all the specified
        /// errors.</returns>
        /// <exception cref="ArgumentNullException">Errors cannot be null.</exception>
        internal static DbDataException CreateException(IList<DbError> errors)
        {
            Oliviann.ADP.CheckArgumentNull(errors, nameof(errors));

            var builder = new StringBuilder();
            foreach (DbError error in errors)
            {
                if (builder.Length > 0)
                {
                    builder.Append(Environment.NewLine);
                }

                builder.AppendFormat("{0} [{1}] {2}", error.Number, error.State, error.Message);
            }

            return new DbDataException(builder.ToString(), errors);
        }
    }
}