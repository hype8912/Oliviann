namespace Oliviann.Tests.Net.Mail
{
    #region Usings

    using System;
    using System.IO;
    using System.Net.Mail;
    using Oliviann.Net.Mail;
    using Oliviann.Testing.Fixtures;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Xunit;
    using Assert = Xunit.Assert;

    #endregion Usings

    [Trait("Category", "CI")]
    [DeploymentItem(@"TestObjects\EmptyFile.txt")]
    public class AttachmentCollectionExtensionsTests : IClassFixture<PathCleanupFixture>
    {
        #region Fields

        private readonly PathCleanupFixture fixture;

        #endregion

        #region Constructor/Destructor

        public AttachmentCollectionExtensionsTests(PathCleanupFixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion

        /// <summary>
        /// Verifies a null collection throws an argument null exception.
        /// </summary>
        [Fact]
        public void AttachmentCollectionAddTest_NullCollection()
        {
            AttachmentCollection col = null;
            Assert.Throws<ArgumentNullException>(() => col.Add(@"c:\temp\log.txt"));
        }

        /// <summary>
        /// Verifies a bad file name throws an argument exception.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AttachmentCollectionAddTest_BadFileNames(string inputFileName)
        {
            var msg = new MailMessage();
            Assert.Throws<ArgumentNullException>(() => msg.Attachments.Add(inputFileName));
        }

        /// <summary>
        /// Verifies a valid path is added to the collection.
        /// </summary>
        [Fact]
        public void AttachmentCollectionAddTest_ValidItem()
        {
            string testPath = this.fixture.CurrentDirectory + @"\TestObjects\EmptyFile.txt";
            Assert.True(File.Exists(testPath));

            var msg = new MailMessage();
            msg.Attachments.Add(testPath);

            Assert.Single(msg.Attachments);
        }
    }
}