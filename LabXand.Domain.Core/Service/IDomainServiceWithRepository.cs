using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.DomainLayer.Core
{
    public interface IDomainServiceWithRepository<TDomain, TRepository, TIdentifier> : IDomainService<TDomain, TIdentifier>, IReadOnlyDomainWithRepositoryService<TDomain, TRepository, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
        where TRepository : IRepository<TDomain, TIdentifier>
    {

    }
}
