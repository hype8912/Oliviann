namespace Oliviann.Web.Tests.TestObjects
{
    #region Usings

    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// Represents a
    /// </summary>
    public class OrderAttributes
    {
        #region Properties

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "unit_price")]
        public double UnitPrice { get; set; }

        [DataMember(Name = "quantity")]
        public double Quantity { get; set; }

        #endregion Properties
    }
}