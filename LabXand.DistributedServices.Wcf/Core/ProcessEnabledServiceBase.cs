using LabXand.DomainLayer.Core;
using LabXand.Security.Core;
using LabXand.Logging.Core;
using LabXand.DistributedServices.Process;

namespace LabXand.DistributedServices.Wcf
{
    public abstract class ProcessEnabledServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TUserContext, TLogEntry>
        : SecurityEnabledServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TUserContext, TLogEntry>
        where TDomainEntity : DomainEntityBase<TIdentifier>
        where TDomainService : IDomainService<TDomainEntity, TIdentifier>
        where TUserContext : class, IUserContext
        where TLogEntry : class, IRootLogEntry
    {

        public ProcessEnabledServiceBase(IProcessEnabledUnitOfWork unitOfWork, TDomainService domainService, IEntityMapper<TDomainEntity, TDomainDto> mapper, IUserContextDetector<TUserContext> userContextDetector, ILogContext<TLogEntry> logContext)
            : base(unitOfWork, domainService, mapper, userContextDetector, logContext)
        {
        }

    }
}
