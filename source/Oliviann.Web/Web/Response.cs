namespace Oliviann.Web
{
    #region Usings

    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// Represents a basic API response object.
    /// </summary>
    [DataContract]
    [XmlRoot(ElementName = "response")]
    [Serializable]
    public class Response<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the HTTP status code for the response.
        /// </summary>
        [DataMember(Name = "status")]
        [JsonProperty(PropertyName = "status")]
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        [DataMember(Name = "message")]
        [JsonProperty(PropertyName = "message")]
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the result for the response.
        /// </summary>
        [DataMember(Name = "data")]
        [JsonProperty(PropertyName = "data")]
        [XmlElement(ElementName = "data")]
        public T Data { get; set; }

        #endregion
    }
}