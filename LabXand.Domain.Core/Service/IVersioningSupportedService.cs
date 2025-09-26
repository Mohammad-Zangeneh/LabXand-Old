using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface IVersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository> : IDomainServiceWithRepository<TDomain, TRepository, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>, IVersioningSupportedEntity<TIdentifier, TVersion>
        where TRepository : IRepository<TDomain, TIdentifier>
    {
        TDomain GetLatestVersion(TDomain domain);
    }
}
