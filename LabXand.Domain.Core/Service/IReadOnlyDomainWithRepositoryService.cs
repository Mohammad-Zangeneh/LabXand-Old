using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface IReadOnlyDomainWithRepositoryService<TDomain, TRepository, TIdentifier> : IReadOnlyDomainService<TDomain,TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
        where TRepository : IRepository<TDomain, TIdentifier>
    {
        TRepository Repository { get; }
    }
}
