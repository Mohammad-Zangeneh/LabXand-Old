using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Core
{
    public enum LogEntryTypes
    {
        Database = 1,
        Process = 2
    }
    public enum OperationTypes
    {
        NoChange = 0,
        Add = 1,
        Modified = 2,
        Delete = 3
    }
    public enum ServiceOperationTypes
    {
        Fetch = 1,
        Save = 2,
        Delete = 3,
        Other = 4
    }
    public enum ServiceOperationStatus
    {
        Success = 1,
        Error = 2
    }
}
