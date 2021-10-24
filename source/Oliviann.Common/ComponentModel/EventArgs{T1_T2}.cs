namespace Oliviann.ComponentModel
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a 2-event argument.
    /// </summary>
    /// <typeparam name="T1">The type of the event arguments first component.
    /// </typeparam>
    /// <typeparam name="T2">The type of the event arguments second component.
    /// </typeparam>
    [Serializable]
    public class EventArgs<T1, T2> : EventArgs<T1>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T1, T2}"/>
        /// class.
        /// </summary>
        /// <param name="value1">The value of the event arguments first
        /// component.</param>
        /// <param name="value2">The value of the event arguments second
        /// component.</param>
        public EventArgs(T1 value1, T2 value2) : base(value1) => this.Value2 = value2;

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the value of the <see cref="EventArgs{T1, T2}"/> object's
        /// second component.
        /// </summary>
        public T2 Value2 { get; }

        #endregion Properties
    }
}