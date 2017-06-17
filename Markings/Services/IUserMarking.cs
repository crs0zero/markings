using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Markings.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserMarking" in both code and config file together.
    [ServiceContract]
    public interface IUserMarking
    {
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "get?userid={userid}")]
        ResultSet<Model.UserMarking> Get(string userid);
    }
}
