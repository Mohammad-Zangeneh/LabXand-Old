using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Elasticsearch
{
   public class DailyReport:DocumentReport
    {
        public DailyReport()
        {

        }

        public DailyReport(string key, long documentCount, double? totalRequestLength, double? totalResponseLength)
        {
            TotalRequestLength = totalRequestLength;
            TotalResponseLength = totalResponseLength;
            Key = key;
            DocumentCount = documentCount;
        }

        public double? TotalRequestLength { get; set; }
        public double? TotalResponseLength { get; set; }
        public double ErrorCount { get; set; }
    }
}
