using LabXand.DomainLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    public abstract class ServiceValidatiorBase<TDomain, TIdentifier> : IServiceValidator<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        public bool IsValid(TDomain domain)
        {
            List<string> brokenRules = BrokenRules(domain);
            if (brokenRules.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                brokenRules.ForEach(S => stringBuilder.AppendLine(S));
                throw new ViolationServiceRuleException(stringBuilder.ToString());
            }
            return true;
        }

        public abstract List<string> BrokenRules(TDomain domain);
    }
}
