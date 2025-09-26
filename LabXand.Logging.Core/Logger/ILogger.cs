namespace LabXand.Logging.Core
{
    public interface ILogger
    {
        int Log(ApiLogEntry logEntry);
    }

    public interface ILogEntry
    {
        string Description { get; }
    }

    public interface IRootLogEntry : ILogEntry
    {
        void AppendDescription(string  description);
        void SetDetails(ILogEntry detailsEntry);
        void ExceptionOccured(string message, ExceptionInformation exception);
    }
}
