using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Markings.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFile" in both code and config file together.
    [ServiceContract]
    public interface IFile
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "get?id={id}")]
        ResultSet<Model.File> Get(string id);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "list?id={id}")]
        ResultSet<Model.File> List(string id);
    }
}
