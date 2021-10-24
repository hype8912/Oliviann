namespace Oliviann.ComponentModel
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a 1-event argument.
    /// </summary>
    /// <typeparam name="T1">The type of the event arguments only component.
    /// </typeparam>
    [Serializable]
    public class EventArgs<T1> : EventArgs
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T1}"/> class.
        /// </summary>
        /// <param name="value">The value of the event arguments only component.
        /// </param>
        public EventArgs(T1 value) => this.Value1 = value;

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the value of the <see cref="EventArgs{T1}"/> object's first
        /// component.
        /// </summary>
        public T1 Value1 { get; }

        #endregion Properties
    }
}