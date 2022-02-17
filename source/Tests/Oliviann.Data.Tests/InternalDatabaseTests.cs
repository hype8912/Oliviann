namespace Oliviann.Data.Tests
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class InternalDatabaseTests
    {
        /// <summary>
        /// Verifies the default instances of class.
        /// </summary>
        [Fact]
        public void InternalDatabaseTest_DefaultValues()
        {
            var db = new InternalDatabase();

            Assert.Null(db.Name);
            Assert.Equal(DatabaseProvider.None, db.DatabaseType);
            Assert.Null(db.Location);
            Assert.Equal(0, db.Port);
            Assert.Null(db.UserName);
            Assert.Null(db.Password);
            Assert.Null(db.SystemDatabase);

            Assert.Null(db.ToString());
        }

        /// <summary>
        /// Verifies setting the values returns the correct value.
        /// </summary>
        [Fact]
        public void InternalDatabaseTest_SetGetValues()
        {
            var db = new InternalDatabase
                         {
                             Name = "Annotation Tool",
                             DatabaseType = DatabaseProvider.MicrosoftSqlServer,
                             Location = @"localhost\IMIS\AT_DB",
                             Port = 1433,
                             UserName = "TheBigUser",
                             Password = "MagicBeansWork",
                             SystemDatabase = @"C:\Temp\system.mdw"
                         };

            Assert.Equal("Annotation Tool", db.Name);
            Assert.Equal(DatabaseProvider.MicrosoftSqlServer, db.DatabaseType);
            Assert.Equal(@"localhost\IMIS\AT_DB", db.Location);
            Assert.Equal(1433, db.Port);
            Assert.Equal("TheBigUser", db.UserName);
            Assert.Equal("MagicBeansWork", db.Password);
            Assert.Equal(@"C:\Temp\system.mdw", db.SystemDatabase);

            Assert.Equal(db.Name, db.ToString());
        }

        /// <summary>
        /// Verifies the cloned database values are the same and then verifies
        /// they are different.
        /// </summary>
        [Fact]
        public void InternalDatabaseCloneTest_ValuesAreSameThenDifferent()
        {
            IDatabase origDb = new InternalDatabase
            {
                DatabaseType = DatabaseProvider.MicrosoftSqlServer,
                Location = @"C:\Users\ik510e\Source\Repos\ES OPTIMUS\Optimus\bin",
                Name = "Optimus",
                Password = "aPassword",
                Port = 1536,
                UserName = "Big Spur",
                SystemDatabase = @"C:\Temp\master.mdw"
            };

            var cloneDb = (IDatabase)origDb.Clone();

            Assert.Equal(cloneDb.DatabaseType, origDb.DatabaseType);
            Assert.Equal(cloneDb.Location, origDb.Location);
            Assert.Equal(cloneDb.Name, origDb.Name);
            Assert.Equal(cloneDb.Password, origDb.Password);
            Assert.Equal(cloneDb.Port, origDb.Port);
            Assert.Equal(cloneDb.UserName, origDb.UserName);
            Assert.Equal(cloneDb.SystemDatabase, origDb.SystemDatabase);

            origDb.Name = "PMDB";
            origDb.DatabaseType = DatabaseProvider.SQLite;
            origDb.Password = "1234";
            origDb.Port = 222;
            origDb.UserName = "ClothAtTheFair";
            origDb.Location = @"C:\Users\ik510e\Source\Repos\ES OPTIMUS\Optimus";
            origDb.SystemDatabase = @"C:\Temp\temp.mdb";

            Assert.NotEqual(cloneDb.Name, origDb.Name);
            Assert.NotEqual(cloneDb.Location, origDb.Location);
            Assert.NotEqual(cloneDb.Password, origDb.Password);
            Assert.NotEqual(cloneDb.Port, origDb.Port);
            Assert.NotEqual(cloneDb.UserName, origDb.UserName);
            Assert.NotEqual(cloneDb.DatabaseType, origDb.DatabaseType);
            Assert.NotEqual(cloneDb.SystemDatabase, origDb.SystemDatabase);
        }
    }
}