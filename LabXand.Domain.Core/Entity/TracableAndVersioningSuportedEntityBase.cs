using LabXand.Security.Core;

namespace LabXand.DomainLayer.Core
{
    public abstract class TracableAndVersioningSuportedEntity<TIdentifier, TVersion, TTraceData, TUserContext> : ITracableAndVersioningSuportedEntity<TIdentifier, TVersion, TTraceData, TUserContext>
        where TTraceData : ITraceData
        where TUserContext : IUserContext
    {
    }
}
