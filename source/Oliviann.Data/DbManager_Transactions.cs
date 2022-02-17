namespace Oliviann.Data
{
    #region Usings

    using System.Data;

    #endregion Usings

    /// <summary>
    /// Represents a partial class to the main DbManager class for handling
    /// transactions.
    /// </summary>
    public sealed partial class DbManager
    {
        #region Fields

        /// <summary>
        /// Gets the transaction instance.
        /// </summary>
        /// <value>The transaction.</value>
        public IDbTransaction Transaction { get; internal set; }

        #endregion Fields

        #region Transactions

        /// <summary>
        /// Starts a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="level">The isolation level under which the transaction
        /// should run.</param>
        public void BeginTransaction(IsolationLevel level)
        {
            this.Open();
            if (this.Transaction == null)
            {
                this.Transaction = this.Connection.BeginTransaction(level);
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            this.Transaction?.Commit();
            this.Transaction = null;
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void RollbackTransaction()
        {
            this.Transaction?.Rollback();
            this.Transaction = null;
        }

        #endregion Transactions
    }
}