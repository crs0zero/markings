using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Markings.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Marking" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Marking.svc or Marking.svc.cs at the Solution Explorer and start debugging.
    public class Marking : IMarking
    {
        public ResultSet<Model.Marking> Get(string id)
        {
            return new ResultSet<Model.Marking>(Model.Marking.Load(Guid.Parse(id)));
        }

        public ResultSet<Model.Marking> List(int offset, int limit)
        {
            Model.Marking[] markings = Model.Marking.List(offset,limit);
            return new ResultSet<Model.Marking>(markings.ToList<Model.Marking>(), Model.Marking.Count());
            // return Newtonsoft.Json.JsonConvert.SerializeObject(m);
        }
    }
}
