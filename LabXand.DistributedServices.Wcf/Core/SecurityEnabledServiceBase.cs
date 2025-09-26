using LabXand.DomainLayer.Core;
using LabXand.Infrastructure.Data;
using LabXand.Security.Core;
using LabXand.Logging.Core;

namespace LabXand.DistributedServices.Wcf
{
    public abstract class SecurityEnabledServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TUserContext, TLogEntry> : ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>
        where TDomainEntity : DomainEntityBase<TIdentifier>
        where TDomainService : IDomainService<TDomainEntity, TIdentifier>
        where TUserContext : class, IUserContext
        where TLogEntry : class, IRootLogEntry
    {
        public SecurityEnabledServiceBase(IUnitOfWork unitOfWork, TDomainService domainService, IEntityMapper<TDomainEntity, TDomainDto> mapper, IUserContextDetector<TUserContext> userContextDetector, ILogContext<TLogEntry> logContext)
            : base(unitOfWork, domainService, mapper, logContext)
        {
            _userContextDetector = userContextDetector;
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

        protected override string GetUserInformation()
        {
            if (UserContext != null)
            {
                return UserContext.ToString();
            }
            return string.Empty;
        }
    }    
}
