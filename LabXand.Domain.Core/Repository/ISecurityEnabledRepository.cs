using LabXand.Security.Core;

namespace LabXand.DomainLayer.Core
{
    public interface ISecurityEnabledRepository<TDomain, TIdentifier, TUserContext> : IRepository<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
        where TUserContext : IUserContext
    {
        TUserContext UserContext { get; }
    }

}
