using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Elasticsearch
{
    public class SystemDetails
    {
        public SystemDetails()
        {
            SubsystemDetails = new List<SubsystemDetails>();
            OrganizationDetails = new List<SubsystemDetails>();
        }
        public IList<SubsystemDetails> SubsystemDetails { get; set; }
        public IList<SubsystemDetails> OrganizationDetails { get; set; }
        public IList<DocumentReport> BrowserDetails { get; set; }
        public IList<DailyReport> DialyDetails { set; get; }
        public IList<DocumentReport> StatusCodeDetails { set; get; }
        public double? TotalRequestLength { get; set; }
        public double? TotalResponsetLength { get; set; }
        public double? TotalElapsedTime { get; set; }
        public long TotalDocument { get; set; }
        public long TotalError{ get; set; }
    }
}
