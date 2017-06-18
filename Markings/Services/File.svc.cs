using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Markings.Model;

namespace Markings.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "File" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select File.svc or File.svc.cs at the Solution Explorer and start debugging.
    public class File : IFile
    {
        public ResultSet<Model.File> Get(string id)
        {
            throw new NotImplementedException();
        }

        public ResultSet<Model.File> List(string id)
        {
            Guid? folderid = null;
            if (id != "null")
            {
                folderid = Guid.Parse(id);
            }
            return new ResultSet<Model.File>(Model.File.List(folderid).ToList<Model.File>());
        }
    }
}
