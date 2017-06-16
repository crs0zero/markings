using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Markings
{
    [System.Runtime.Serialization.DataContract]
    public class ResultSet<T>
    {
        [System.Runtime.Serialization.DataMember]
        public int total_items;

        [System.Runtime.Serialization.DataMember]
        public int count;

        [System.Runtime.Serialization.DataMember]
        public System.Collections.Generic.List<T> items;

        public ResultSet(T obj)
        {
            total_items = 1;
            count = 1;
            items = new List<T>(1);
            items.Add(obj);
        }
        public ResultSet(System.Collections.Generic.List<T> results, int total)
        {
            total_items = total;
            count = results.Count;
            items = results;
        }
    }
}