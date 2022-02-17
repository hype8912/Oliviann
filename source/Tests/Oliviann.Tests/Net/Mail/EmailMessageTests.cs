namespace Oliviann.Tests.Net.Mail
{
    #region Usings

    using Oliviann.Net.Mail;
    using Xunit;

    #endregion Usings

    [Trait("Category", "CI")]
    public class EmailMessageTests
    {
        /// <summary>
        /// Verifies a default instance has the correct values.
        /// </summary>
        [Fact]
        public void EmailMessageTest_DefaultcTor()
        {
            var msg = new EmailMessage();

            Assert.Null(msg.Id);
            Assert.Null(msg.FromAddress);
            Assert.Empty(msg.ToAddresses);
            Assert.Empty(msg.CcAddresses);
            Assert.Empty(msg.BccAddresses);
            Assert.Null(msg.Subject);
            Assert.Null(msg.Body);
            Assert.Empty(msg.Attachments);
            Assert.Null(msg.HostAddress);
            Assert.Null(msg.HostPort);
        }
    }
}