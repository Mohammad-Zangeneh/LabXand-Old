using LabXand.Security.Core;

namespace LabXand.DomainLayer.Core
{
    public interface ITracableEntity<TUserContext>
    {
        void SetTraceData(TUserContext userContext);
    }
    public interface ITracableEntity<TTraceData, TUserContext> : ITracableEntity<TUserContext>
        where TTraceData : ITraceData
        where TUserContext : IUserContext
    {
        TTraceData TraceData { get; }
    }
}
