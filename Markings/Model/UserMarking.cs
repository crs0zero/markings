using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Markings.Model
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    [System.Runtime.Serialization.DataContract]
    public class UserMarking
    {
        [System.Runtime.Serialization.DataMember]
        string user_id;

        [System.Runtime.Serialization.DataMember]
        Guid marking_id;

        private UserMarking(System.Data.DataRow record)
        {
            user_id = (string)record["UserId"];
            marking_id = (Guid)record["MarkingId"];
        }

        private UserMarking(string userid, Guid marking)
        {
            this.user_id = userid;
            this.marking_id = marking;
        }

        public static UserMarking Create(string userid, Guid marking_id)
        {
            Guid id = Guid.NewGuid();
            System.Data.SqlClient.SqlParameter[] args = {
                new System.Data.SqlClient.SqlParameter("@USERID", userid),
                new System.Data.SqlClient.SqlParameter("@MARKING", marking_id),
            };
            if ((int)Database.exec("INSERT INTO Markings (UserId, MarkingId) VALUES (@USERID, @MARKING)", args) > 0)
                return new UserMarking(userid, marking_id);
            return null;
        }

        public static int Count(string userid)
        {
            System.Data.SqlClient.SqlParameter[] args = { new System.Data.SqlClient.SqlParameter("@USERID", userid) };
            return (int)Database.query_value("SELECT Count(*) FROM UserMarking where UserId = @USERID");
        }

        // lists assets that are found within a scanset
        // this may need to be refactored to utilize paging operations but for now just return the entire list
        /*
        public static Marking[] List(int offset = 0, int pagesize = 0)
        {
            System.Data.SqlClient.SqlParameter[] args = new System.Data.SqlClient.SqlParameter[0];
            string sql = Database.paginate("SELECT * FROM Markings ORDER BY Label", offset, pagesize, ref args);
            System.Data.DataTable data = (System.Data.DataTable)Database.query(sql, args);
            Marking[] list = new Marking[data.Rows.Count];
            for (int i = 0; i < data.Rows.Count; i++)
            {
                list[i] = new Marking(data.Rows[i]);
            }
            return list;
        }
        */

        public static UserMarking[] Load(string userid)
        {
            System.Data.SqlClient.SqlParameter[] args = { new System.Data.SqlClient.SqlParameter("@USERID", userid) };
            System.Data.DataTable data= (System.Data.DataTable)Database.query("SELECT * FROM UserMarking where UserId = @USERID", args);
            if (data.Rows.Count > 0)
            {
                UserMarking[] markings = new UserMarking[data.Rows.Count];
                for (int i = 0; i < data.Rows.Count; i++)
                    markings[i] = new UserMarking(data.Rows[i]);
                return markings;
            }
            return null;
        }


    }
}
