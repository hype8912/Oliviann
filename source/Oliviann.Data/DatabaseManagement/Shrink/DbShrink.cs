namespace Oliviann.Data.DatabaseManagement.Shrink
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Class for shrinking a database.
    /// </summary>
    public class DbShrink : IDbShrink
    {
        #region Contsructor/Deconstructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DbShrink"/> class.
        /// </summary>
        /// <param name="databaseObject">The database object.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="databaseObject" /> is <c>null</c>.</exception>
        public DbShrink(IDatabase databaseObject)
        {
            Oliviann.ADP.CheckArgumentNull(databaseObject, @"databaseObject");
            this.Database = databaseObject;
        }

        #endregion Contsructor/Deconstructor

        #region Properties

        /// <summary>
        /// Gets the database object.
        /// </summary>
        /// <value>The database object.</value>
        public IDatabase Database { get; private set; }

        #endregion Properties

        /// <summary>
        /// Shrinks the specified database.
        /// </summary>
        /// <param name="decrypter">A function for taking in an encrypted string
        /// and returning a decrypted string.</param>
        /// <returns>
        ///   <c>True</c> if the database shrinks successfully; otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool ShrinkDatabase(Func<string, string> decrypter)
        {
            IDbShrink agent = DbShrinkFactory.CreateShrinkAgent(this.Database);
            return agent != null && agent.ShrinkDatabase(decrypter);
        }
    }
}