#if NETFRAMEWORK

namespace Oliviann.Data.Tests.CRUD
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlServerCe;
    using System.Linq;
    using Oliviann.Common.TestObjects.Xml;
    using Oliviann.Data.Tests.TestObjects;
    using Oliviann.Testing.Fixtures;
    using Dapper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "DB")]
    [DeploymentItem(@"TestObjects\TestDbSqlCompact.sdf")]
    public class SqlCompactDapperCrudTests : SqlCompactTestsBase
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCompactDapperCrudTests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public SqlCompactDapperCrudTests(PathCleanupFixture fixture) : base(fixture)
        {
        }

        #endregion Constructor/Destructor

        #region Create Tests

        /// <summary>
        /// Verifies creating a user in the SQL Compact database returns a
        /// result of true.
        /// </summary>
        [Fact]
        public void SqlCompactTest_CreatePerson()
        {
            Person user = this.CreateRandomPerson();
            bool result = this.CreateUser(user);

            Assert.True(result);
        }

        /// <summary>
        /// Common helper method for creating a user in the database.
        /// </summary>
        private bool CreateUser(Person user)
        {
            const string Sql =
                @"INSERT INTO People
                  (Uid, LastName, FirstName, MiddleInitial, PhoneNumber, EmailAddress, Status)
                  VALUES (@Uid, @LastName, @FirstName, @MiddleInitial, @PhoneNumber, @EmailAddress, @Status)";

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            var parms = new { Uid = user.Id, user.LastName, user.FirstName, user.MiddleInitial, user.PhoneNumber, user.EmailAddress, user.Status };

            bool result = manager.ExecuteNonQueryWrapper(Sql, parms);
            return result;
        }

        /// <summary>
        /// Verifies creating a new book that we can get the generated database
        /// key back.
        /// </summary>
        /// <remarks>Because SQL Compact can't run chained queries and the
        /// IDENTITY call has to be made with the same connection instance you
        /// have to manage the calls to Dapper manually.</remarks>
        [Fact]
        public void SqlCompactTest_CreateBook_GetIdentity()
        {
            const string Sql = @"INSERT INTO Books(Author, Title, Year) VALUES (@Author, @Title, @Year)";

            var bk = new Book { Author = "NavPress", Title = "How to find the one", Year = 1996 };

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            var parms = new { bk.Author, bk.Title, bk.Year };

            manager.Open();
            int count = manager.Connection.Execute(Sql, parms, commandType: CommandType.Text);

            int key = 0;
            if (count > 0)
            {
                key = manager.Connection.QuerySingle<int>("SELECT @@IDENTITY");
            }

            manager.Close();
            Assert.True(key >= 2);
        }

        #endregion Create Tests

        #region Read Tests

        /// <summary>
        /// Verifies creating a user in the SQL Compact database is the exact
        /// same when it read from the database.
        /// </summary>
        [Fact]
        public void SqlCompactTest_ReadPerson()
        {
            Person user = this.CreateRandomPerson();
            this.CreateUser(user);

            const string Sql =
                @"SELECT Uid as Id, LastName, FirstName, MiddleInitial, PhoneNumber, EmailAddress, Status
                  FROM [People]
                  WHERE Uid = @Uid";

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            IEnumerable<Person> people = manager.ExecuteReaderWrapper<Person>(Sql, new { Uid = user.Id });

            Assert.NotNull(people);
            Assert.Single(people);

            Person resultPerson = people.First();
            Assert.Equal(user.Id, resultPerson.Id);
            Assert.Equal(user.LastName, resultPerson.LastName);
            Assert.Equal(user.FirstName, resultPerson.FirstName);
            Assert.Equal(user.MiddleInitial, resultPerson.MiddleInitial);
            Assert.Equal(user.PhoneNumber, resultPerson.PhoneNumber);
            Assert.Equal(user.EmailAddress, resultPerson.EmailAddress);
            Assert.Equal(user.Status, resultPerson.Status);
        }

        #endregion Read Tests

        #region Update Tests

        /// <summary>
        /// Verifies creating a user in the SQL Compact database and then
        /// updating the user has the correct results when read from the
        /// database.
        /// </summary>
        [Fact]
        public void SqlCompactTest_UpdatePerson()
        {
            // Create new user in database.
            Person user = this.CreateRandomPerson();
            this.CreateUser(user);

            // Update the user in the database.
            user.EmailAddress = "magicunicorn@gmail.com";

            const string SqlUpdate =
                @"UPDATE [People]
                  SET EmailAddress = @EmailAddress
                  WHERE Uid = @Uid";

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            var parms = new { Uid = user.Id, user.EmailAddress };

            bool result = manager.ExecuteNonQueryWrapper(SqlUpdate, parms);
            manager.Dispose();

            // Read the results from the database to ensure the user is updated.
            const string SqlRead =
                @"SELECT Uid as Id, LastName, FirstName, MiddleInitial, PhoneNumber, EmailAddress, Status
                  FROM [People]
                  WHERE Uid = @Uid";

            manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            IEnumerable<Person> people = manager.ExecuteReaderWrapper<Person>(SqlRead, new { Uid = user.Id });

            Assert.NotNull(people);
            Assert.Single(people);

            Person resultPerson = people.First();
            Assert.Equal(user.Id, resultPerson.Id);
            Assert.Equal(user.LastName, resultPerson.LastName);
            Assert.Equal(user.FirstName, resultPerson.FirstName);
            Assert.Equal(user.MiddleInitial, resultPerson.MiddleInitial);
            Assert.Equal(user.PhoneNumber, resultPerson.PhoneNumber);
            Assert.Equal(user.EmailAddress, resultPerson.EmailAddress);
            Assert.Equal(user.Status, resultPerson.Status);
        }

        #endregion Update Tests

        #region Delete Tests

        /// <summary>
        /// Verifies deleting a user from a SQL Compact database returns a
        /// result of true.
        /// </summary>
        [Fact]
        public void SqlCompactTest_DeletePerson()
        {
            Person user = this.CreateRandomPerson();
            this.CreateUser(user);

            const string Sql =
                @"DELETE FROM [People]
                  WHERE Uid = @Uid";

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            bool result = manager.ExecuteNonQueryWrapper(Sql, new { Uid = user.Id });

            Assert.True(result);
        }

        #endregion Delete Tests
    }
}

#endif