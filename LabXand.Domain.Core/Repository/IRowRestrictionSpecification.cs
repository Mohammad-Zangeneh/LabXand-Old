using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface IRowRestrictionSpecification<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        Expression<Func<TDomain, bool>> Get();
    }
}
