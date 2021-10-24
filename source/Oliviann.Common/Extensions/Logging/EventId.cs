#if NET35 || NET40

namespace Oliviann.Extensions.Logging
{
    /// <summary>
    /// Represents a logging event.
    /// </summary>
    public struct EventId
    {
        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventId" /> struct.
        /// </summary>
        /// <param name="id">The numeric identifier for the event.</param>
        /// <param name="name">The name of this event.</param>
        public EventId(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the numeric identifier for this event.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name of this event.
        /// </summary>
        public string Name { get; }

        #endregion
    }
}

#endif