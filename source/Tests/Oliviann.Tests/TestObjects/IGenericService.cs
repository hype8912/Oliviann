namespace Oliviann.Tests.TestObjects
{
    #region Usings

    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;

    #endregion Usings

    /// <summary>
    /// Represents a
    /// </summary>
    [ServiceContract]
    public interface IGenericService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetString")]
        Message GetString();
    }
}