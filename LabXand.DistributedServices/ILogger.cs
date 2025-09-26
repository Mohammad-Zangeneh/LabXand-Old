using LabXand.DomainLayer.Core;
using LabXand.Infrastructure.Data;
using LabXand.Security.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DistributedServices
{
    

    //public abstract class LoggerBase<TUserContext, TLogEntry> : ILogger<TUserContext>
    //    where TUserContext : IUserContext
    //    where TLogEntry : class,LogEntryBase, new()
    //{
    //    protected ILogerDevice _loggerDevice;
    //    public LoggerBase(ILogerDevice loggerDevice)
    //    {
    //        _loggerDevice = loggerDevice;
    //    }
    //    TLogEntry PrepareEntry<TDomain, TIdentifier>(TLogEntry logEntry, TDomain domain, TUserContext userContext);
    //    TLogEntry PrepareEntry(TLogEntry logEntry, Exception exception, TUserContext userContext);
    //    public void Log<TDomain, TIdentifier>(string operationName, TDomain domain, TUserContext userContext) where TDomain : class, IDomainEntity<TIdentifier>
    //    {
    //        TLogEntry logEntry = new TLogEntry();
    //        logEntry.OperationName = operationName;
    //        _loggerDevice.Write(PrepareEntry<TDomain, TIdentifier>(logEntry, domain, userContext));
    //    }

    //    public void Log(string operationName, Exception exception, TUserContext userContext)
    //    {
    //        TLogEntry logEntry = new TLogEntry();
    //        logEntry.OperationName = operationName;
    //        _loggerDevice.Write(PrepareEntry(logEntry, exception, userContext));
    //    }
    //}

    //public class Logger<TUserContext> : LoggerBase<TUserContext, LogEntryBase>
    //{

    //}    

    
    //public enum OperationTypes
    //{
    //    Add = 1,
    //    Modified = 2,
    //    Delete = 3
    //}
    //public abstract class DetailsLogEntry
    //{
    //    public int Id { get; set; }
    //    /// <summary>
    //    /// Database
    //    /// Workflow
    //    /// </summary>
    //    public LogEntryTypes Type { get; set; }
    //    public string Title { get; set; }
    //}
    //public class DataBaseLogEntry : DetailsLogEntry
    //{
    //    public DataBaseLogEntry()
    //    {
    //        this.Type = LogEntryTypes.Database;
    //    }
    //    public string DomainName { get; set; }
    //    public string IdentityValue { get; set; }
    //    public OperationTypes OperationType { get; set; }
    //    public string Description { get; set; }
    //}

    //public class ProcessLogEntry : DetailsLogEntry
    //{
    //    public string ResultMessage { get; set; }
    //    public List<KeyValuePair<string,string>> Variables { get; set; }
    //}
}
