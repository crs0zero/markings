using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Markings.Model
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    [System.Runtime.Serialization.DataContract]
    public class Marking
    {
        [System.Runtime.Serialization.DataMember]
        Guid id;

        [System.Runtime.Serialization.DataMember]
        string label;

        private Marking(System.Data.DataRow record)
        {
            id = (Guid)record["Id"];
            label = (string)record["Label"];
        }

        private Marking(Guid id, string label)
        {
            this.id = id;
            this.label = label;
        }

        public static Marking Create(string label)
        {
            Guid id = Guid.NewGuid();
            System.Data.SqlClient.SqlParameter[] args = {
                new System.Data.SqlClient.SqlParameter("@ID", id),
                new System.Data.SqlClient.SqlParameter("@LABEL", label),
            };
            if ((int)Database.exec("INSERT INTO Marking (Id, Label) VALUES (@ID, @LABEL)", args) > 0)
                return new Marking(id, label);
            return null;
        }

        public static int Count()
        {
            return (int)Database.query_value("SELECT Count(*) FROM Markings");
        }

        // lists assets that are found within a scanset
        // this may need to be refactored to utilize paging operations but for now just return the entire list
        public static Marking[] List(int offset = 0, int pagesize = 0)
        {
            System.Data.SqlClient.SqlParameter[] args = new System.Data.SqlClient.SqlParameter[0];
            string sql = Database.paginate("SELECT * FROM Marking ORDER BY Label", offset, pagesize, ref args);
            System.Data.DataTable data = (System.Data.DataTable)Database.query(sql, args);
            Marking[] list = new Marking[data.Rows.Count];
            for (int i = 0; i < data.Rows.Count; i++)
            {
                list[i] = new Marking(data.Rows[i]);
            }
            return list;
        }

        public static Marking Load(Guid id)
        {
            System.Data.SqlClient.SqlParameter[] args = { new System.Data.SqlClient.SqlParameter("@MAKRINGID", id) };
            System.Data.DataTable data= (System.Data.DataTable)Database.query("SELECT * FROM Marking WHERE Id = @MARKINGID", args);

            if (data.Rows.Count == 1)
            {
                return new Marking(data.Rows[0]);
            }
            return null;
        }


    }
}
