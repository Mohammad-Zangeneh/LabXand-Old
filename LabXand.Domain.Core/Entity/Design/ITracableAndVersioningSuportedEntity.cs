using LabXand.Security.Core;

namespace LabXand.DomainLayer.Core
{
    public interface ITracableAndVersioningSuportedEntity<TIdentifier, TVersion, TTraceData, TUserContext> : IVersioningSupportedEntity<TIdentifier, TVersion>, ITracableEntity<TTraceData, TUserContext>
        where TTraceData : ITraceData
        where TUserContext : IUserContext
    {
    }
}
