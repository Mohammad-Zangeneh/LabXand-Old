using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LabXand.DomainLayer.Core
{
    public interface IServiceValidator<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        bool IsValid(TDomain domain);
        List<string> BrokenRules(TDomain domain);
    }    

}
