using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Markings.Model
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    [System.Runtime.Serialization.DataContract]
    public class File
    {
        [System.Runtime.Serialization.DataMember]
        Guid id;

        [System.Runtime.Serialization.DataMember]
        Guid? parent;

        [System.Runtime.Serialization.DataMember]
        string name;

        [System.Runtime.Serialization.DataMember]
        Guid marking_id;

        private File(System.Data.DataRow record)
        {
            id = (Guid)record["Id"];
            name = (string)record["Name"];
            if (record["Parent"].GetType() == typeof(System.DBNull))
                parent = null;
            else
                parent = (Guid)record["Parent"];
            marking_id = (Guid)record["MarkingId"];
        }

        private File(Guid id, string filename, Guid parent, Guid marking_id)
        {
            this.id = id;
            this.parent = parent;
            this.name = filename;
            this.marking_id = marking_id;
        }

        public static File Create(string filename, Guid parent, Guid marking_id)
        {
            Guid id = Guid.NewGuid();
            System.Data.SqlClient.SqlParameter[] args = {
                new System.Data.SqlClient.SqlParameter("@ID", id),
                new System.Data.SqlClient.SqlParameter("@PARENT", parent),
                new System.Data.SqlClient.SqlParameter("@FILENAME", filename),
            };
            if ((int)Database.exec("INSERT INTO File (Id, Parent, Filename) VALUES (@ID, @PARENT, @FILENAME)", args) > 0)
                return new File(id, filename, parent, marking_id);
            return null;
        }

        public static int Count(Guid folderid)
        {
            return (int)Database.query_value("SELECT Count(*) FROM Markings");
        }

        // lists assets that are found within a scanset
        // this may need to be refactored to utilize paging operations but for now just return the entire list
        public static File[] List(Guid? folderid)
        {
            System.Data.DataTable data;
            if (folderid != null)
            {
                System.Data.SqlClient.SqlParameter[] args = { new System.Data.SqlClient.SqlParameter("@PARENT", folderid) };
                data = (System.Data.DataTable)Database.query("SELECT * FROM [File] WHERE Parent = @PARENT ORDER BY Name", args);
            }
            else
                data = (System.Data.DataTable)Database.query("SELECT * FROM [File] WHERE Parent IS NULL ORDER BY Name");
            File[] list = new File[data.Rows.Count];
            for (int i = 0; i < data.Rows.Count; i++)
            {
                list[i] = new File(data.Rows[i]);
            }
            return list;
        }
    }
}
