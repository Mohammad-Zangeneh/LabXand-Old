using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    [Serializable]
    public class ViolationServiceRuleException : Exception
    {
        public ViolationServiceRuleException()
        {
        }
        public ViolationServiceRuleException(string message) : base(message) { }
        public ViolationServiceRuleException(string message, Exception inner) : base(message, inner) { }
        protected ViolationServiceRuleException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context) { }
        public object RelatedDomainEntity { get; set; }
        public List<string> BrokenRules { get; set; }
    }
}
