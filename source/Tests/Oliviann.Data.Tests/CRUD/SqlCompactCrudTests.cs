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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Testing.Fixtures;
    using Oliviann.Data.Tests.TestObjects;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "DB")]
    [DeploymentItem(@"TestObjects\TestDbSqlCompact.sdf")]
    public class SqlCompactCrudTests : SqlCompactTestsBase
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCompactCrudTests"/>
        /// class.
        /// </summary>
        /// <param name="fixture">The current fixture.</param>
        public SqlCompactCrudTests(PathCleanupFixture fixture) : base(fixture)
        {
        }

        #endregion Constructor/Destructor

        #region Create Tests

        /// <summary>
        /// Verifies creating a user in the SQL Compact database returns a
        /// result of 1.
        /// </summary>
        [Fact]
        public void SqlCompactTest_CreatePerson()
        {
            Person user = this.CreateRandomPerson();
            int result = this.CreateUser(user);

            Assert.Equal(1, result);
        }

        /// <summary>
        /// Common helper method for creating a user in the database.
        /// </summary>
        private int CreateUser(Person user)
        {
            const string Sql =
                @"INSERT INTO People
                  (Uid, LastName, FirstName, MiddleInitial, PhoneNumber, EmailAddress, Status)
                  VALUES (@Uid, @LastName, @FirstName, @MiddleInitial, @PhoneNumber, @EmailAddress, @Status)";

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            manager.AddParameter("@Uid", user.Id);
            manager.AddParameter("@LastName", user.LastName);
            manager.AddParameter("@FirstName", user.FirstName);
            manager.AddParameter("@MiddleInitial", user.MiddleInitial);
            manager.AddParameter("@PhoneNumber", user.PhoneNumber);
            manager.AddParameter("@EmailAddress", user.EmailAddress);
            manager.AddParameter("@Status", user.Status);

            int result = manager.ExecuteNonQuery(CommandType.Text, Sql);
            manager.Close();

            return result;
        }

        /// <summary>
        /// Verifies creating a new book that we can get the generated database
        /// key back.
        /// </summary>
        [Fact]
        public void SqlCompactTest_CreateBook_GetIdentity()
        {
            const string Sql = @"INSERT INTO Books(Author, Title, Year) VALUES (@Author, @Title, @Year)";

            var bk = new Book { Author = "NavPress", Title = "How to find the one", Year = 1996 };

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            manager.AddParameter("@Author", bk.Author);
            manager.AddParameter("@Title", bk.Title);
            manager.AddParameter("@Year", bk.Year);

            int count = manager.ExecuteNonQuery(CommandType.Text, Sql);
            int key = 0;
            if (count > 0)
            {
                manager.ClearParameters();
                key = manager.ExecuteScalar(CommandType.Text, "SELECT @@IDENTITY").ToInt32();
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
                @"SELECT Uid, LastName, FirstName, MiddleInitial, PhoneNumber, EmailAddress, Status
                  FROM [People]
                  WHERE Uid = @Uid";

            Func<IDataReader, Person> mapper =
                reader => new Person
                    {
                        Id = reader["Uid"].ToInt32(),
                        LastName = (string)reader["LastName"],
                        FirstName = (string)reader["FirstName"],
                        MiddleInitial = (string)reader["MiddleInitial"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        EmailAddress = (string)reader["EmailAddress"],
                        Status = (bool)reader["Status"],
                    };

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            manager.AddParameter("@Uid", user.Id);
            List<Person> people = manager.ExecuteReaderWrapper(Sql, null, null, mapper);

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
            manager.AddParameter("@Uid", user.Id);
            manager.AddParameter("@EmailAddress", user.EmailAddress);
            int result = manager.ExecuteNonQuery(CommandType.Text, SqlUpdate);
            manager.Dispose();

            // Read the results from the database to ensure the user is updated.
            const string SqlRead =
                @"SELECT Uid, LastName, FirstName, MiddleInitial, PhoneNumber, EmailAddress, Status
                  FROM [People]
                  WHERE Uid = @Uid";

            Func<IDataReader, Person> mapper =
                reader => new Person
                {
                    Id = reader["Uid"].ToInt32(),
                    LastName = (string)reader["LastName"],
                    FirstName = (string)reader["FirstName"],
                    MiddleInitial = (string)reader["MiddleInitial"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    EmailAddress = (string)reader["EmailAddress"],
                    Status = (bool)reader["Status"],
                };

            manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            manager.AddParameter("@Uid", user.Id);
            List<Person> people = manager.ExecuteReaderWrapper(SqlRead, null, null, mapper);

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
        /// result of 1.
        /// </summary>
        [Fact]
        public void SqlCompactTest_DeletePerson()
        {
            Person user = this.CreateRandomPerson();

            try
            {
                this.CreateUser(user);
            }
            catch
            {
            }

            const string Sql =
                @"DELETE FROM [People]
                  WHERE Uid = @Uid";

            var manager = new DbManager(new SqlCeProviderFactory()) { ConnectionString = this.GetConnectionString() };
            manager.AddParameter("@Uid", user.Id);

            int result = manager.ExecuteNonQuery(CommandType.Text, Sql);
            manager.Close();

            Assert.Equal(1, result);
        }

        #endregion Delete Tests
    }
}

#endif