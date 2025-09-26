using LabXand.Core;
using LabXand.DomainLayer.Core;
using LabXand.DomainLayer;
using LabXand.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using LabXand.Core.Validation;
using LabXand.Logging.Core;

namespace LabXand.DistributedServices.Wcf
{
    public abstract class ApplicationServiceBase<TDomainEntity, TDomainDto, TDomainService, TIdentifier, TLogEntry> : IApplicationService<TDomainEntity, TDomainDto, TDomainService, TIdentifier>
        where TDomainEntity : DomainEntityBase<TIdentifier>
        where TDomainService : IDomainService<TDomainEntity, TIdentifier>
        where TLogEntry : class, IRootLogEntry
    {
        public ApplicationServiceBase(IUnitOfWork unitOfWork, TDomainService domainService, IEntityMapper<TDomainEntity, TDomainDto> mapper, ILogContext<TLogEntry> logContext)
        {
            UnitOfWork = unitOfWork;
            DomainService = domainService;
            Mapper = mapper;
            LogContext = logContext;
        }
        protected IUnitOfWork UnitOfWork { get; private set; }
        protected TDomainService DomainService { get; private set; }
        protected IEntityMapper<TDomainEntity, TDomainDto> Mapper { get; private set; }
        protected ILogContext<TLogEntry> LogContext { get; private set; }

        protected ServiceLogEntry LogEntry { get; private set; }

        #region Fetch
        public TDomainDto Find(Criteria criteria)
        {
            return InvokeFetchMethod<TDomainDto>(() => Mapper.MapTo(DomainService.Find(criteria)));
            //return Mapper.MapTo(DomainService.Find(criteria));
        }
        public Paginated<TDomainDto> FindPage(Criteria criteria, List<SortItem> sortItems, int page, int size)
        {
            return InvokeFetchMethod<Paginated<TDomainDto>>(() => FindPage(DomainService.FindPage(criteria, sortItems, page, size)));
            //IPagedList<TDomainEntity> result = DomainService.FindPage(criteria, sortItems, page, size);

            //return FindPage(result);

        }
        public Paginated<TDomainDto> FindPage(IPagedList<TDomainEntity> result)
        {
            //return InvokeFetchMethod<Paginated<TDomainDto>>(() => new Paginated<TDomainDto>(new PagedList<TDomainDto>(DtoSelector(result), result.PageIndex, result.PageSize, result.TotalCount)));
            return new Paginated<TDomainDto>(new PagedList<TDomainDto>(DtoSelector(result), result.PageIndex, result.PageSize, result.TotalCount));
        }
        public IList<TDomainDto> FindAll(Criteria criteria, List<SortItem> sortItems)
        {
            //return DtoSelector(DomainService.FindAll(criteria, sortItems));
            return InvokeFetchMethod<IList<TDomainDto>>(() => DtoSelector(DomainService.FindAll(criteria, sortItems)));
        }
        public int Count(Criteria criteria)
        {
            return Invoke<int>(() => DomainService.Count(criteria), ServiceOperationTypes.Fetch);
            //return DomainService.Count(criteria);
        }
        #endregion

        #region Save
        public virtual TDomainDto Save(TDomainDto domainDto)
        {
            return Save(domainDto, true);
        }
        public virtual TDomainDto Save(TDomainDto domainDto, bool selfCommitOperation)
        {
            return InvokeSaveMethod<TDomainDto>(() => InternalSave(domainDto, selfCommitOperation));
            //return InternalSave(domainDto, selfCommitOperation);
        }

        private TDomainDto InternalSave(TDomainDto domainDto, bool selfCommitOperation)
        {
            ValidationHelper.Validate(domainDto);
            var domainEntity = Mapper.CreateFrom(domainDto);
            DomainService.Save(domainEntity);

            if (selfCommitOperation)
            {
                if (LogEntry != null)//morsa
                    LogEntry.Details = UnitOfWork.Commit();
                else
                    UnitOfWork.Commit();
            }

            return Mapper.MapTo(domainEntity);
        }
        #endregion

        #region Delete
        public virtual bool Delete(TDomainDto domainDto)
        {
            return InvokeDeleteMethod<bool>(() => InternalDelete(domainDto));
            //return InternalDelete(domainDto);
        }
        private bool InternalDelete(TDomainDto domainDto)
        {
            DomainService.Remove(Mapper.CreateFrom(domainDto));
            //LogEntry.Details = UnitOfWork.Commit();

            if (LogEntry != null)//morsa
                LogEntry.Details = UnitOfWork.Commit();
            else
                UnitOfWork.Commit();
            return true;
        }
        #endregion

        protected List<TDomainDto> DtoSelector(IList<TDomainEntity> result)
        {
            if (result != null)
                return result.Select(D => Mapper.MapTo(D)).ToList();
            return new List<TDomainDto>();
        }

        #region Invoke Methodes
        protected T InvokeFetchMethod<T>(Func<T> method)
        {
            return Invoke<T>(method, ServiceOperationTypes.Fetch);
        }

        protected T InvokeSaveMethod<T>(Func<T> method)
        {
            return Invoke<T>(method, ServiceOperationTypes.Save);
        }

        protected T InvokeDeleteMethod<T>(Func<T> method)
        {
            return Invoke<T>(method, ServiceOperationTypes.Delete);
        }

        protected T InvokeOtherMethod<T>(Func<T> method)
        {
            return Invoke<T>(method, ServiceOperationTypes.Other);
        }

        protected T Invoke<T>(Func<T> method, ServiceOperationTypes operationType)
        {
            BeforeInvoke(operationType);
            T result = method();
            AfterInvoke();
            return result;
        }
        #endregion        
        protected virtual void AfterInvoke()
        {
            LogEntry.Success();
            LogContext.Current.SetDetails(LogEntry);
        }

        protected virtual void BeforeInvoke(ServiceOperationTypes operationType)
        {
            LogEntry = CreateLogEntry(operationType);
            LogEntry.SetUserInformation(GetUserInformation());
        }

        protected virtual string GetUserInformation()
        {
            return string.Empty;
        }

        protected ServiceLogEntry CreateLogEntry(ServiceOperationTypes operationType)
        {
            ServiceLogEntry logEntry = new ServiceLogEntry(operationType);
            logEntry.Type = operationType;
            logEntry.Initiate(LogContext.Current, new Guid(UnitOfWork.InstanceKey));
            logEntry.SetMethodParameters(6);
            LogContext.Current.SetDetails(LogEntry);
            return logEntry;
        }
    }

}
