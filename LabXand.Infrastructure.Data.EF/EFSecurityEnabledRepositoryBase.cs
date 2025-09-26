using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace LabXand.Infrastructure.Data.EF
{
    public abstract class EFSecurityEnabledRepositoryBase<TDomain, TIdentifier, TUserContext> : EFRepositoryBase<TDomain, TIdentifier>, ISecurityEnabledRepository<TDomain, TIdentifier, TUserContext>
        where TDomain : DomainEntityBase<TIdentifier>
        where TUserContext : class,IUserContext
    {
        public EFSecurityEnabledRepositoryBase(IDataContext dataContext, IUserContextDetector<TUserContext> userContextDetector)
            : base(dataContext)
        {
            _userContextDetector = userContextDetector;
        }
        protected IUserContextDetector<TUserContext> _userContextDetector;
        public TUserContext UserContext
        {
            get
            {
                if (_userContextDetector != null)
                    return _userContextDetector.UserContext;
                return null;
            }
        }
    }
}
