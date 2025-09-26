using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LabXand.Core
{
    [DataContract]
    public class Paginated<T> : IPaginated<T>
    {
        public Paginated(IPagedList<T> pagedList)
        {
            this.Data = pagedList;
            this.TotalCount = pagedList.TotalCount;
        }
        [DataMember]
        public IEnumerable<T> Data { get; set; }

        [DataMember]
        /// <summary>
        /// Gets the total number of data.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
