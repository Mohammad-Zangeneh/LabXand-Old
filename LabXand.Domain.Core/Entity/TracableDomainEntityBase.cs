using LabXand.DomainLayer.Core;
using LabXand.Security.Core;

namespace LabXand.DomainLayer.Core
{
    public abstract class TracableDomainEntityBase<TIdentifier, TTraceData, TUserContext> : DomainEntityBase<TIdentifier>, ITracableEntity<TTraceData, TUserContext>
        where TTraceData : ITraceData
        where TUserContext : IUserContext
    {
        public TracableDomainEntityBase()
        {
            TraceData = CreateNewTraceData();
        }        
        
        public TTraceData TraceData { get; private set; }

        public abstract void SetTraceData(TUserContext userContext);
        protected abstract TTraceData CreateNewTraceData();
    }
}
