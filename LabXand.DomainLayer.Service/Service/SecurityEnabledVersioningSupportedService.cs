using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace LabXand.DomainLayer
{
    public abstract class SecurityEnabledVersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository, TUserContext> : VersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository>, ISecurityEnabledReadOnlyService<TDomain, TIdentifier, TUserContext>
        where TDomain : DomainEntityBase<TIdentifier>, IVersioningSupportedEntity<TIdentifier, TVersion>
        where TRepository : class, IRepository<TDomain, TIdentifier>
        where TUserContext : class, IUserContext
    {
        public SecurityEnabledVersioningSupportedService(TRepository repository, IUserContextDetector<TUserContext> userContextInitializer)
            : base(repository)
        {
            _userContextDetector = userContextInitializer;
        }
        IUserContextDetector<TUserContext> _userContextDetector;
        public TUserContext UserContext
        {
            get
            {
                if (_userContextDetector != null)
                    return _userContextDetector.UserContext;
                return null;
            }
        }
         
        public override TDomain Save(TDomain domain)
        {
            if (domain is ITracableEntity<TUserContext>)
            {
                ((ITracableEntity<TUserContext>)domain).SetTraceData(UserContext);
            }

            return base.Save(domain);
        }
    }
}
