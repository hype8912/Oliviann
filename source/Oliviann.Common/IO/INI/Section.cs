namespace Oliviann.IO
{
    #region Usings

    using System.Collections.Generic;
    using System.Linq;
    using Oliviann.Collections;

    #endregion Usings

    /// <summary>
    /// Defines an INI file section for holding all the properties.
    /// </summary>
    public class Section
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        /// <param name="name">The section name.</param>
        public Section(string name)
        {
            this.Name = name;
            this.Properties = new List<KeyValuePair<string, string>>();
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the section name.
        /// </summary>
        /// <value>The section name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the properties for this section.
        /// </summary>
        /// <value>The properties for this section.</value>
        public List<KeyValuePair<string, string>> Properties { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Checks if the property already exist and then either
        /// updates it to the current value or adds a new parameter
        /// to the list.
        /// </summary>
        /// <param name="key">The parameter key.</param>
        /// <param name="value">The parameter value.</param>
        public void AddProperty(string key, string value)
        {
            KeyValuePair<string, string> parm = this.Properties.FirstOrDefault(p => p.Key == key);
            if (!parm.Equals(default(KeyValuePair<string, string>)))
            {
                this.Properties.Remove(parm);
                this.Properties.Add(parm.New(value));
                return;
            }

            this.Properties.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// Gets the parameter value for the specified parameter key.
        /// </summary>
        /// <param name="key">The parameter key.</param>
        /// <returns>A string value for the specified parameter key.</returns>
        public string GetPropertyValue(string key)
        {
            KeyValuePair<string, string> parm = this.Properties.FirstOrDefault(p => p.Key == key);
            return parm.Equals(default(KeyValuePair<string, string>)) ? string.Empty : parm.Value;
        }

        #endregion Methods

        #region Overrides

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion Overrides
    }
}