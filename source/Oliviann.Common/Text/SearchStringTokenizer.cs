namespace Oliviann.Text
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Oliviann.Collections.Generic;

    #endregion Usings

    /// <summary>
    /// Represents a search query string into tokens. Looks for parameterized
    /// strings, quoted strings, and plain text.
    /// </summary>
    public class SearchStringTokenizer
    {
        #region Fields

        /// <summary>
        /// The regular expression for finding parameterized quoted strings.
        /// </summary>
        private static readonly Regex ParameterizedQuotedString = new Regex(@"[a-zA-Z0-9]+:"".+""", RegexOptions.Compiled);

        /// <summary>
        /// The regular expression for finding parameterized non-quoted strings.
        /// </summary>
        private static readonly Regex ParameterizedString = new Regex(@"[a-zA-Z0-9]+(:|: )[a-zA-Z0-9_,.-]+", RegexOptions.Compiled);

        /// <summary>
        /// The regular expression for finding quoted strings.
        /// </summary>
        private static readonly Regex QuotedString = new Regex(@"""[a-zA-Z0-9!@#$%^&*()-_+= ]+""", RegexOptions.Compiled);

        /// <summary>
        /// Place holder for the original query string passed in.
        /// </summary>
        private readonly string rawQueryString;

        /// <summary>
        /// The temporary query string used for parsing.
        /// </summary>
        private string queryString;

        /// <summary>
        /// The non-parameterized count number for the collection entries.
        /// </summary>
        private int nonParameterEntryCount;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SearchStringTokenizer" /> class.
        /// </summary>
        /// <param name="queryText">The search query text.</param>
        public SearchStringTokenizer(string queryText)
        {
            this.rawQueryString = queryText;
            this.Tokens = new Dictionary<string, string>();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the collection of tokens after they have been parsed.
        /// </summary>
        /// <value>
        /// The collection of parsed tokens.
        /// </value>
        public IDictionary<string, string> Tokens { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Parses this instance.
        /// </summary>
        public void Parse()
        {
            if (!this.ValidateParameters())
            {
                return;
            }

            this.Initialize();

            bool containsParams = this.queryString.Contains(':');
            bool containsQuotes = this.queryString.Contains('"');
            if (!containsParams && !containsQuotes)
            {
                // String doesn't contain parameters or quotes.
                this.ParseSingleString();
            }
            else if (containsQuotes && !containsParams)
            {
                // String contains quotes but not parameters.
                this.ParseQuoteString();
            }
            else
            {
                this.ParseParameterizedString(containsQuotes);
            }
        }

        #endregion Methods

        #region Helpers

        /// <summary>
        /// Validates the input parameters for the parser.
        /// </summary>
        /// <returns>True if all the input parameters are valid; otherwise,
        /// false.</returns>
        private bool ValidateParameters()
        {
            return !this.rawQueryString.IsNullOrWhiteSpace();
        }

        /// <summary>
        /// Initializes all the variables for the parser.
        /// </summary>
        private void Initialize()
        {
            this.nonParameterEntryCount = 0;
            this.Tokens.Clear();

            if (this.rawQueryString.StartsWith(' ') || this.rawQueryString.EndsWith(' '))
            {
                this.queryString = this.rawQueryString.Trim();
            }
            else
            {
                this.queryString = this.rawQueryString;
            }
        }

        /// <summary>
        /// Parses a string of words that doesn't contain any parameters or
        /// quotes.
        /// </summary>
        private void ParseSingleString()
        {
            string[] strings = this.queryString.Split(StringSplitOptions.RemoveEmptyEntries, ' ');

            foreach (string s in strings)
            {
                this.Tokens.Add(this.nonParameterEntryCount.ToString(), s);
                this.nonParameterEntryCount += 1;
            }
        }

        /// <summary>
        /// Parses a string of words that doesn't contain any parameters.
        /// </summary>
        private void ParseQuoteString()
        {
            MatchCollection matches = QuotedString.Matches(this.queryString);

            foreach (Match m in matches)
            {
                this.Tokens.Add(this.nonParameterEntryCount.ToString(), m.Value);
                this.nonParameterEntryCount += 1;
                this.queryString = this.queryString.Replace(m.Value, " ");
            }

            if (matches.Count > 0)
            {
                this.RemoveDoubleSpacesFromQueryString();
            }

            if (this.queryString.Length > 0)
            {
                this.ParseSingleString();
            }
        }

        /// <summary>
        /// Parses a string of words containing parameters and values.
        /// </summary>
        /// <param name="containsQuotes">True if the current query string
        /// contains quotes.</param>
        private void ParseParameterizedString(bool containsQuotes)
        {
            if (containsQuotes)
            {
                this.ParseParameterizedQuotedString();
            }

            MatchCollection matches = ParameterizedString.Matches(this.queryString);
            foreach (Match m in matches)
            {
                KeyValuePair<string, string> pair = this.SplitParameterString(m.Value);
                this.Tokens.AddOrUpdate(pair.Key, pair.Value);
                this.queryString = this.queryString.Replace(m.Value, " ");
            }

            if (matches.Count > 0)
            {
                this.RemoveDoubleSpacesFromQueryString();
            }

            this.ParseQuoteString();
        }

        /// <summary>
        /// Parses a string of words containing parameters and quoted values.
        /// </summary>
        private void ParseParameterizedQuotedString()
        {
            MatchCollection matches = ParameterizedQuotedString.Matches(this.queryString);

            foreach (Match m in matches)
            {
                KeyValuePair<string, string> pair = this.SplitParameterString(m.Value);
                this.Tokens.AddOrUpdate(pair.Key, pair.Value);
                this.queryString = this.queryString.Replace(m.Value, " ");
            }

            if (matches.Count > 0)
            {
                this.RemoveDoubleSpacesFromQueryString();
            }
        }

        /// <summary>
        /// Removes the double whitespace from query string and replaces them
        /// with single spaces.
        /// </summary>
        private void RemoveDoubleSpacesFromQueryString()
        {
            while (this.queryString.Contains("  "))
            {
                this.queryString = this.queryString.Replace("  ", " ");
            }
        }

        /// <summary>
        /// Splits the parameter string into separate entries.
        /// </summary>
        /// <param name="paramString">The combined parameter string.</param>
        /// <returns>A pair with the parameter as the key and the parameter
        /// value as the value.</returns>
        private KeyValuePair<string, string> SplitParameterString(string paramString)
        {
            int colonIndex = paramString.IndexOf(':');
            string parm = paramString.Substring(0, colonIndex);
            string value = paramString.Substring(colonIndex + 1);

            return new KeyValuePair<string, string>(parm, value);
        }

        #endregion Helpers
    }
}