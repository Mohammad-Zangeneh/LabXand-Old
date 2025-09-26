using System;
using System.Runtime.Serialization;

namespace LabXand.Logging.Core
{
    [Serializable]
    [DataContract]
    public class DataBaseLogEntry : DetailsLogEntry
    {
        public DataBaseLogEntry()
        {
            this.Type = LogEntryTypes.Database;
        }
        [DataMember]
        public string DomainName { get; set; }
        [DataMember]
        public string IdentityValue { get; set; }
        [DataMember]
        public OperationTypes OperationType { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

}
