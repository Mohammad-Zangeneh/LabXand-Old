using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LabXand.Logging.Core
{
    [DataContract]
    [Serializable]
    public class ProcessLogEntry : DetailsLogEntry
    {
        public string ResultMessage { get; set; }
        public List<KeyValuePair<string, string>> Variables { get; set; }
    }
}
