namespace Oliviann.Text
{
    #region Usings

    using System.Text.RegularExpressions;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used helpers for working with
    /// Regular Expressions.
    /// </summary>
    public static class RegularExpressions
    {
        #region Fields

        /// <summary>
        /// Regular Expression pattern for validating an email address.
        /// </summary>
        /// <remarks>
        /// The <see cref="Regex"/> expression used for this comparison can be
        /// found
        /// <see url="http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx">
        /// here</see>.
        /// </remarks>
        public const string EmailValidationPattern =
            @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        /// <summary>
        /// Regular Expression instance for validating an email address.
        /// </summary>
        public static readonly Regex EmailValidationExpression =
            new Regex(EmailValidationPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Regular Expression instance for validating alpha characters only.
        /// </summary>
        public static readonly Regex AlphaCharactersRegEx = new Regex(@"^[a-zA-Z]*$", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expression instance for validating alpha and whitespace characters only.
        /// </summary>
        public static readonly Regex AlphaSpaceCharactersRegEx = new Regex(@"^[a-zA-Z\s]*$", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expression instance for validating an capital letters.
        /// </summary>
        public static readonly Regex CapitalLettersRegEx = new Regex(@"([A-Z])", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expression instance for validating an INI file line
        /// key/value pair.
        /// </summary>
        public static readonly Regex IniKeyValueLine = new Regex(@"[a-zA-Z0-9]+=", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expression instance for validating an INI file line section
        /// group. Ex. [SECTION]
        /// </summary>
        public static readonly Regex IniSectionLine = new Regex(@"\[[a-zA-Z0-9]+\]", RegexOptions.Compiled);

        /// <summary>
        /// Regular Expression instance for validating an number.
        /// </summary>
        public static readonly Regex NumbersRegEx = new Regex(@"(?=.*\d)", RegexOptions.Compiled);

        #endregion Fields
    }
}