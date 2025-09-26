using LabXand.Extensions;
using LabXand.DomainLayer.Core;
using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    public abstract class UniquenessValidationBase<TDomain, TIdentifier> : ServiceValidatiorBase<TDomain, TIdentifier>
            where TDomain : DomainEntityBase<TIdentifier>
    {
        public UniquenessValidationBase(IReadOnlyDomainService<TDomain, TIdentifier> service)
            : this(service, "Id")
        {
        }
        public UniquenessValidationBase(IReadOnlyDomainService<TDomain, TIdentifier> service, string idPropertyName) : this(service, idPropertyName, service.Count)
        {
        }
        public UniquenessValidationBase(IReadOnlyDomainService<TDomain, TIdentifier> service, string idPropertyName, Func<Expression<Func<TDomain, bool>>, int> countMethod)
        {
            _idPropertyName = idPropertyName;
            Service = service;
            _CountMethod = countMethod;
        }

        protected string _idPropertyName;
        protected Func<Expression<Func<TDomain, bool>>, int> _CountMethod;
        public IReadOnlyDomainService<TDomain, TIdentifier> Service { get; set; }
    }
}
