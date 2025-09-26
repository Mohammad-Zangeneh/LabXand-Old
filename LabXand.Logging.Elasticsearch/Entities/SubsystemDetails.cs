using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Elasticsearch
{
    public class SubsystemDetails: DocumentReport
    {
        public double? SumOfRequest { get; set; }
        public double? SumOfResponse { get; set; }
        public double? SumOfElapsedTime { get; set; }
        public long ErrorCount { get; set; }
    }
}
