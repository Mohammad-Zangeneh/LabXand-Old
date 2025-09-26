using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface ISecurityEnabledReadOnlyService<TDomain, TIdentifier, TUserContext> : IReadOnlyDomainService<TDomain, TIdentifier>, ISecurityEnabledService<TUserContext>
        where TDomain : DomainEntityBase<TIdentifier>
        where TUserContext : IUserContext
    {

    }
}
