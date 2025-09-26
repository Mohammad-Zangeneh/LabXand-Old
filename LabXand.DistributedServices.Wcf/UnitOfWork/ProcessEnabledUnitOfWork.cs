using LabXand.DistributedServices.Process;
using LabXand.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Transactions;
using LabXand.Logging.Core;

namespace LabXand.DistributedServices.Wcf
{

    public class ProcessEnabledUnitOfWork : IProcessEnabledUnitOfWork
    {
        public ProcessEnabledUnitOfWork(IUnitOfWork dataContext)
        {
            InstanceKey = Guid.NewGuid().ToString();
            DataContext = dataContext;
        }

        public IUnitOfWork DataContext { get; private set; }

        public string InstanceKey { get; private set; }

        protected List<Action> runAfterCommitActions = new List<Action>();
        TransactionScope _transactionScop = null;
        public List<DataBaseLogEntry> Commit()
        {
            try
            {
                var result = DataContext.Commit();
                runAfterCommitActions.ForEach(t => t.Invoke());
                RunInTransaction(true);
                return result;
            }
            catch (Exception)
            {
                RunInTransaction(false);
                throw;
            }
        }

        public void RoleBack()
        {
            throw new NotImplementedException();
        }

        public void StartTransaction()
        {
            _transactionScop = new TransactionScope();
        }

        private void RunInTransaction(bool isCommmit)
        {
            if (_transactionScop != null)
            {
                if (isCommmit)
                    _transactionScop.Complete();
                _transactionScop.Dispose();
            }
        }

        public IUnitOfWork RunAfterDataBaseCommit(Action action)
        {
            runAfterCommitActions.Add(action);
            return this;
        }
    }
}
