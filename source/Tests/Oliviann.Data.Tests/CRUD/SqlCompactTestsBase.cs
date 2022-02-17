namespace Oliviann.Data.Tests.CRUD
{
    #region Usings

    using System;
    using System.Security;
    using Oliviann.Data.Tests.TestObjects;
    using Oliviann.Security;
    using Oliviann.Testing.Fixtures;
    using Xunit;

    #endregion

    public abstract class SqlCompactTestsBase : IClassFixture<PathCleanupFixture>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCompactTestsBase"/>
        /// class.
        /// </summary>
        protected SqlCompactTestsBase(PathCleanupFixture fixture)
        {
            this.Fixture = fixture;
        }

        #endregion

        #region Properties

        protected PathCleanupFixture Fixture { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new person object with random data.
        /// </summary>
        /// <returns>A newly created person.</returns>
        protected Person CreateRandomPerson()
        {
            var rand = new Random((int)DateTime.UtcNow.Ticks);
            return new Person
                {
                    Id = rand.Next(1, int.MaxValue),
                    LastName = StringHelpers.GenerateRandomString(25),
                    FirstName = StringHelpers.GenerateRandomString(20),
                    MiddleInitial = StringHelpers.GenerateRandomString(1),
                    PhoneNumber = StringHelpers.GenerateRandomString(12),
                    EmailAddress = StringHelpers.GenerateRandomString(100),
                    Status = rand.Next(1, int.MaxValue).IsEven()
                };
        }

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <returns>The database connection string.</returns>
        protected SecureString GetConnectionString()
        {
            string location = this.Fixture.CurrentDirectory + @"\TestObjects\TestDbSqlCompact.sdf";
            ////this.fixture.DeletePaths.Add(location);
            return $"Data Source={location}; Persist Security Info=False".ToSecureString();
        }

        #endregion
    }
}