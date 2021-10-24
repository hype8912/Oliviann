namespace Oliviann.ComponentModel
{
    #region Usings

    using System;

    #endregion Usings

    /// <summary>
    /// Represents a 3-event argument.
    /// </summary>
    /// <typeparam name="T1">The type of the event arguments first component.
    /// </typeparam>
    /// <typeparam name="T2">The type of the event arguments second component.
    /// </typeparam>
    /// <typeparam name="T3">The type of the event arguments third component.
    /// </typeparam>
    [Serializable]
    public class EventArgs<T1, T2, T3> : EventArgs<T1, T2>
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EventArgs{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="value1">The value of the event arguments first
        /// component.</param>
        /// <param name="value2">The value of the event arguments second
        /// component.</param>
        /// <param name="value3">The value of the event arguments third
        /// component.</param>
        public EventArgs(T1 value1, T2 value2, T3 value3) : base(value1, value2) => this.Value3 = value3;

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the value of the <see cref="EventArgs{T1, T2, T3}"/> object's
        /// third component.
        /// </summary>
        public T3 Value3 { get; }

        #endregion Properties
    }
}