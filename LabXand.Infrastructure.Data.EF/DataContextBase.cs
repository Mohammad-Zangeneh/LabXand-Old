using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using System.Text;
using LabXand.Extensions;
using LabXand.DomainLayer.Core;
using LabXand.Logging.Core;
using System.Transactions;
using System.Data.Entity.Infrastructure.Interception;

namespace LabXand.Infrastructure.Data.EF
{
    public class DbContextBase : DbContext, IDataContext, IUnitOfWork
    {
        private readonly ILogContext _logContext;
        public DbContextBase(string nameOrConnectionString)
            : this(nameOrConnectionString, new EmptyLogContext())
        {
        }
        public DbContextBase(string nameOrConnectionString, ILogContext logContext)
            : base(nameOrConnectionString)
        {
            InstanceKey = Guid.NewGuid().ToString();
            Configuration.LazyLoadingEnabled = false;
            DbInterception.Add(new StringCleanerInterceptor());
            _logContext = logContext;
        }

        public IDbSet<TEntiy> Entities<TEntiy>() where TEntiy : class
        {
            return this.Set<TEntiy>();
        }
        protected string GetDbEntityValidationErrorMessage(DbEntityValidationException ex)
        {
            StringBuilder message = new StringBuilder();
            foreach (var eve in ex.EntityValidationErrors)
            {
                message.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State));
                foreach (var ve in eve.ValidationErrors)
                {
                    message.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage));
                }
            }

            return message.ToString();
        }

        #region IDataContext
        public string InstanceKey { get; private set; }


        public TEntity Add<TEntity>(TEntity entity) where TEntity : class
        {
            return this.Entities<TEntity>().Add(entity);
        }

        public TEntity Edit<TEntity>(TEntity entity, List<string> navigationPropertiesMustbeUpdate, List<string> constantFields) where TEntity : class
        {
            //TEntity oldEntity = this.EntitySet<TEntity>(navigationPropertiesMustbeUpdate).FirstOrDefault(searchIdPredicate);

            Entities<TEntity>().Attach(entity);
            var entry = this.Entry(entity);

            entry.State = EntityState.Modified;

            if (constantFields != null)
            {
                foreach (var item in constantFields)
                {
                    entry.Property(item).IsModified = false;
                }
            }
            //if (navigationPropertiesMustbeUpdate != null)
            //{
            //    foreach (var item in navigationPropertiesMustbeUpdate)
            //    {
            //        entry.Property(item).IsModified = true;
            //    }
            //}

            return entity;
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            if (this.Entry(entity).State == EntityState.Detached)
            {
                this.Entities<TEntity>().Attach(entity);
            }
            this.Entities<TEntity>().Remove(entity);
        }


        /// <summary>
        /// Return IQueryable As NoTracking.
        /// Just use for read data.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="includeNavigationProperties"></param>
        /// <returns></returns>
        public IQueryable<TEntity> EntitySet<TEntity>(List<string> includeNavigationProperties) where TEntity : class
        {
            return EntitySet<TEntity>(includeNavigationProperties, true);
        }

        public IQueryable<TEntity> EntitySet<TEntity>(List<Expression<Func<TEntity, dynamic>>> includeNavigationProperties) where TEntity : class
        {
            return EntitySet<TEntity>(includeNavigationProperties, true);
        }

        public IQueryable<TEntity> EntitySet<TEntity>(List<Expression<Func<TEntity, dynamic>>> includeNavigationProperties, bool isNoTracking) where TEntity : class
        {
            return EntitySet<TEntity>(includeNavigationProperties.Select(n => ExpressionHelper.GetNameOfProperty(n)).ToList(), true);
        }

        public IQueryable<TEntity> EntitySet<TEntity>(List<string> includeNavigationProperties, bool isNoTracking) where TEntity : class
        {
            IQueryable<TEntity> query = this.Entities<TEntity>();
            query = query.Include(includeNavigationProperties);

            if (isNoTracking)
                return query.AsNoTracking();
            return query;
        }
        public string GetTableName<T>() where T : class
        {
            return this.GetTableName<T>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        public long ExecuteSqlCommand(string sqlCommand, string tableName)
        {
            decimal id = (this as IObjectContextAdapter).ObjectContext.ExecuteStoreQuery<decimal>(sqlCommand, tableName).FirstOrDefault();
            return Convert.ToInt64(id) + 1;
        }


        #endregion

        protected List<Action> runAfterCommitActions = new List<Action>();
        TransactionScope _transactionScop = null;
        public List<DataBaseLogEntry> Commit()
        {
            try
            {
                var result = InternalCommit();
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
            RejectScalarChanges();
            RejectNavigationChanges();
            SaveChanges();
        }

        private void RejectScalarChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        private void RejectNavigationChanges()
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var deletedRelationships = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).Where(e => e.IsRelationship && !this.RelationshipContainsKeyEntry(e));
            var addedRelationships = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Where(e => e.IsRelationship);

            foreach (var relationship in addedRelationships)
                relationship.Delete();

            foreach (var relationship in deletedRelationships)
                relationship.ChangeState(EntityState.Unchanged);
        }

        private bool RelationshipContainsKeyEntry(System.Data.Entity.Core.Objects.ObjectStateEntry stateEntry)
        {
            //prevent exception: "Cannot change state of a relationship if one of the ends of the relationship is a KeyEntry"
            //I haven't been able to find the conditions under which this happens, but it sometimes does.
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var keys = new[] { stateEntry.OriginalValues[0], stateEntry.OriginalValues[1] };
            return keys.Any(key => objectContext.ObjectStateManager.GetObjectStateEntry(key).Entity == null);
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
        private static DataBaseLogEntry CreateLogEntryFromEntity(DbEntityEntry dbEntity)
        {
            IDomainEntity domainEntity = dbEntity.Entity as IDomainEntity;
            if (domainEntity != null)
            {
                DataBaseLogEntry logEntry = new DataBaseLogEntry();
                logEntry.Description = domainEntity.EntityDescriptor();
                logEntry.DomainName = domainEntity.GetType().Name;
                logEntry.IdentityValue = domainEntity.Id != null? domainEntity.Id.ToString(): "Id IS NULL";
                logEntry.OperationType = ConvertEntityStateToOperationType(dbEntity.State);

                return logEntry;
            }
            return null;
        }
       static OperationTypes ConvertEntityStateToOperationType(EntityState state)
        {
            switch (state)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Added:
                    return OperationTypes.Add;
                case EntityState.Deleted:
                    return OperationTypes.Delete;
                case EntityState.Modified:
                    return OperationTypes.Modified;
                default:
                    break;
            }
            return OperationTypes.NoChange;
        }
        public List<DataBaseLogEntry> DumpTrackedEntities(DbContext context)
        {
            List<DataBaseLogEntry> result = new List<DataBaseLogEntry>();

            return context.
                ChangeTracker
                .Entries()
                .Where(t => t.State != EntityState.Detached && t.State == EntityState.Unchanged && t.Entity != null)
                .Select(t => CreateLogEntryFromEntity(t)).ToList();            
        }


        protected List<DataBaseLogEntry> InternalCommit()
        {
            try
            {
                var logs = DumpTrackedEntities(this);
                _logContext.AppendDescription(DbContextExtensions.StringifyDbContextChanges(this));
                SaveChanges();
                return logs;
            }
            catch (DbEntityValidationException ex)
            {
                var newException = new FormattedDbEntityValidationException(ex);
                throw newException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
