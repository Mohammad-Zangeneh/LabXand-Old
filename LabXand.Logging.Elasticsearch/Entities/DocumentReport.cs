using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Elasticsearch
{
    public class DocumentReport
    {
        public string Key { get; set; }
        public long DocumentCount { get; set; }
        public DocumentReport()
        {

        }

        public DocumentReport(string key, long documentCount)
        {
            Key = key;
            DocumentCount = documentCount;
        }
    }
}
