using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Core
{
    [Serializable]
    [DataContract]
    public /*abstract*/ class DetailsLogEntry
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public LogEntryTypes Type { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
}
