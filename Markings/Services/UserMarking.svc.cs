using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Markings.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserMarking" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserMarking.svc or UserMarking.svc.cs at the Solution Explorer and start debugging.
    public class UserMarking : IUserMarking
    {
        public ResultSet<Model.UserMarking> Get(string userid)
        {
            return new ResultSet<Model.UserMarking>(Model.UserMarking.Load(userid).ToList<Model.UserMarking>());
        }
    }
}
