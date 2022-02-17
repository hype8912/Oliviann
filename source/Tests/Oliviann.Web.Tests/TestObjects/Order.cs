namespace Oliviann.Web.Tests.TestObjects
{
    #region Usings

    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Oliviann.Web.Json;
    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    public class Order
    {
        #region Properties

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "customer")]
        public string Customer { get; set; }

        [DataMember(Name = "items")]
        [JsonConverter(typeof(SingleValueArrayConverter<OrderAttributes>))]
        public List<OrderAttributes> Items { get; set; }

        #endregion Properties
    }
}