using LabXand.Logging.Core;
using System;
using System.Collections.Generic;

namespace LabXand.Infrastructure.Data
{
    public interface IUnitOfWork
    {
        string InstanceKey { get; }
        List<DataBaseLogEntry> Commit();
        void RoleBack();
        void StartTransaction();
        IUnitOfWork RunAfterDataBaseCommit(Action action);
    }
}
