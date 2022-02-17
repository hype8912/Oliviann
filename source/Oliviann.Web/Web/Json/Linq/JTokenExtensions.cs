namespace Oliviann.Web.Json.Linq
{
    #region Usings

    using Newtonsoft.Json.Linq;

    #endregion Usings

    /// <summary>
    /// Represents a collection of methods to extend the functionality of
    /// JSON.Net JToken objects.
    /// </summary>
    public static class JTokenExtensions
    {
        /// <summary>
        /// Creates the specified .NET type from the JToken object if not null.
        /// </summary>
        /// <typeparam name="T">The object type that the token will be
        /// deserialized to.</typeparam>
        /// <param name="token">The token to object to retrieve the value from.
        /// </param>
        /// <param name="defaultValue">The default value to be returned when the
        /// token object is null.</param>
        /// <returns>Returns the specified token value with the specified type
        /// if token is not null; otherwise, the default value.</returns>
        /// <remark>This method was added here instead of in Oliviann to keep all
        /// the JSON.Net references to this library.</remark>
        public static T ToObjectSafe<T>(this JToken token, T defaultValue = default)
        {
            return token.GetValueOrDefault(t => t.ToObject<T>(), defaultValue);
        }
    }
}