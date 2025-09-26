using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.DomainLayer.Core
{
    public interface ISecurityEnabledDomainService<TDomain, TIdentifier, TUserContext> : IDomainService<TDomain, TIdentifier>, ISecurityEnabledService<TUserContext>
        where TDomain : DomainEntityBase<TIdentifier>
        where TUserContext : IUserContext
    {

    }
}
