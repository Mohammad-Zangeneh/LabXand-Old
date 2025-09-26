using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Core
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ServiceDesciptorAttribute : Attribute
    {
        public ServiceDesciptorAttribute(ServiceOperationTypes type, string description)
        {
            this.Description = description;
            this.OperationType = type;
        }
        public string Description { get; set; }
        public ServiceOperationTypes OperationType { get; set; }
    }
}
