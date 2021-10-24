namespace Oliviann
{
    /// <summary>
    /// Represents an interface that is <c>updateable</c>.
    /// </summary>
    /// <typeparam name="T">The type of object being implemented.</typeparam>
    public interface IUpdateable<in T>
    {
        /// <summary>Updates this {T} instance with data from the specified
        /// instance.</summary>
        /// <param name="other">The other {T} instance data.</param>
        void Update(T other);
    }
}