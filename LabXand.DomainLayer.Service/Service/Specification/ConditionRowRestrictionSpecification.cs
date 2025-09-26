using LabXand.DomainLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    public class ConditionRowRestrictionSpecification<TDomain, TIdentifier> : IRowRestrictionSpecification<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        Expression<Func<TDomain, bool>> _expression;
        public ConditionRowRestrictionSpecification(Expression<Func<TDomain, bool>> expression)
        {
            _expression = expression;
        }
        public Expression<Func<TDomain, bool>> Get()
        {
            return _expression;
        }
    }
}
