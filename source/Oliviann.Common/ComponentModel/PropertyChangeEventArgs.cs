namespace Oliviann.ComponentModel
{
    #region Usings

    using System.ComponentModel;

    #endregion Usings

    /// <summary>
    /// Provides data for the <see cref="NotifyPropertyChange.PropertyChange"/>
    /// event.
    /// </summary>
    /// <remarks>
    /// A <see cref="NotifyPropertyChange.PropertyChange"/> event is raised when
    /// a property is changed on a component. A <b>PropertyChangeEventArgs</b>
    /// object specifies the name of the property that changed, the value before
    /// it changed, and the new value it changed to.
    /// </remarks>
    public sealed class PropertyChangeEventArgs : PropertyChangedEventArgs
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PropertyChangeEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.
        /// </param>
        /// <param name="oldValue">The old property value.</param>
        /// <param name="newValue">The new property value.</param>
        public PropertyChangeEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the value of the property before it changed.
        /// </summary>
        /// <value>The value of the property before it changed.</value>
        public object OldValue { get; }

        /// <summary>
        /// Gets the value of the property when it changed.
        /// </summary>
        /// <value>The value of the property when it changed.</value>
        public object NewValue { get; }

        #endregion Properties
    }
}