using LabXand.Core;
using LabXand.DomainLayer.Core;
using LabXand.Extensions;
using LabXand.Logging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices.Wcf
{
    public static class ApplicationServiceExtender
    {
        #region Find All
        public static IList<TDomainDto> FindAll<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>(
            this ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> applicationService, Expression<Func<TDomainEntity, bool>> criteria)
            where TDomainEntity : DomainEntityBase<TIdentifier>
            where TDomainService : IDomainService<TDomainEntity, TIdentifier>
            where TLogEntry : class, IRootLogEntry
        {
            return applicationService.FindAll(criteria);
        }

        public static IList<TDomainDto> FindAll<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>(
            this ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> applicationService)
            where TDomainEntity : DomainEntityBase<TIdentifier>
            where TDomainService : IDomainService<TDomainEntity, TIdentifier>
            where TLogEntry : class, IRootLogEntry
        {
            return applicationService.FindAll(null, null);
        }
        #endregion

        #region Find Page

        public static Paginated<TDomainDto> FindPage<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>(
            this ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> applicationService,
            int page, int pageSize)
            where TDomainEntity : DomainEntityBase<TIdentifier>
            where TDomainService : IDomainService<TDomainEntity, TIdentifier>
            where TLogEntry : class, IRootLogEntry
        {
            return applicationService.FindPage(null, null, page, pageSize);
        }

        public static Paginated<TDomainDto> FindPage<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>(
            this ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> applicationService,
            List<SortItem> sortItems, int page, int pageSize)
            where TDomainEntity : DomainEntityBase<TIdentifier>
            where TDomainService : IDomainService<TDomainEntity, TIdentifier>
            where TLogEntry : class, IRootLogEntry
        {
            return applicationService.FindPage(null, sortItems, page, pageSize);
        }

        public static Paginated<TDomainDto> FindPage<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>(
            this ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> applicationService,
            Criteria criteria, int page, int pageSize)
            where TDomainEntity : DomainEntityBase<TIdentifier>
            where TDomainService : IDomainService<TDomainEntity, TIdentifier>
            where TLogEntry : class, IRootLogEntry
        {
            return applicationService.FindPage(criteria, null, page, pageSize);
        }

        #endregion

        #region Find
        public static TDomainDto Find<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>(
            this ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> applicationService, TIdentifier id)
            where TDomainEntity : DomainEntityBase<TIdentifier>
            where TDomainService : IDomainService<TDomainEntity, TIdentifier>
            where TLogEntry : class, IRootLogEntry
        {
            return applicationService.Find(CriteriaBuilder.CreateNew<TDomainEntity>().Equal("Id", id, true));
        }
        public static TDomainDto Find<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry>(
            this ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> applicationService,
            TDomainEntity entity)
            where TDomainEntity : DomainEntityBase<TIdentifier>
            where TLogEntry : class, IRootLogEntry
            where TDomainService : IDomainService<TDomainEntity, TIdentifier>
        {
            return applicationService.Find(entity.Id);
        }
        #endregion
    }
}
