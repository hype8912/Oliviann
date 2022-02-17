namespace Oliviann.Data.Tests.TestObjects
{
    /// <summary>
    /// Represents a person model.
    /// </summary>
    public class Person
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle initial.
        /// </summary>
        /// <value>
        /// The middle initial.
        /// </value>
        public string MiddleInitial { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Person"/> is
        /// active status.
        /// </summary>
        /// <value>
        /// True if active status; otherwise, false.
        /// </value>
        public bool Status { get; set; }

        #endregion Properties
    }
}