namespace Oliviann.Data
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Collects information relevant to a warning or error returned by the
    /// data source.
    /// </summary>
    [Serializable]
    public class DbError
    {
        #region Fields

        /// <summary>
        /// Place holder for the Source property.
        /// </summary>
        private string _source;

        #endregion Fields

        #region Contsructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DbError"/> class.
        /// </summary>
        /// <param name="message">The error description.</param>
        /// <param name="state">The database state.</param>
        /// <param name="number">The data-source specific error number.</param>
        public DbError(string message, string state, int number) : this(@".Net DbClient Data Provider", message, state, number)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbError"/> class.
        /// </summary>
        /// <param name="source">The data driver that produced the error.</param>
        /// <param name="message">The error description.</param>
        /// <param name="state">The database state.</param>
        /// <param name="number">The data-source specific error number.</param>
        public DbError(string source, string message, string state, int number)
        {
            this._source = source;
            this.Number = number;
            this.State = state ?? string.Empty;
            this.Message = message ?? string.Empty;
        }

        #endregion Contsructor/Deconstructor

        #region Properties

        /// <summary>
        /// Gets a short description of the error.
        /// </summary>
        /// <value>A description of the error.</value>
        public string Message { get; }

        /// <summary>
        /// Gets the data source-specific error information.
        /// </summary>
        /// <value>The data source-specific error information.</value>
        public int Number { get; }

        /// <summary>
        /// Gets or sets the name of the driver that generated the error.
        /// </summary>
        /// <value>The name of the driver that generated the error.</value>
        public string Source
        {
            get => this._source ?? string.Empty;
            internal set => this._source = value;
        }

        /// <summary>
        /// Gets the five-character error code that follows the ANSI SQL
        /// standard for the database.
        /// </summary>
        /// <value>The five-character error code, which identifies the source of
        /// the error if the error can be issued from more than one place.
        /// </value>
        public string State { get; }

        #endregion Properties

        #region Overrides

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>A string that represents this instance.</returns>
        public override string ToString() => typeof(DbError) + @": " + this.Message;

        #endregion Overrides
    }
}