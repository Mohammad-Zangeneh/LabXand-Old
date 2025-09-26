namespace LabXand.Logging.Core
{
    public interface ILogContext
    {
        void AppendDescription(string message);
    }
    public interface ILogContext<TEntry> : ILogContext
        where TEntry : class, IRootLogEntry
    {
        void InitiateEntry(TEntry entry);
        TEntry Current { get; }
    }
}
