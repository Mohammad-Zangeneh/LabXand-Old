using LabXand.DomainLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    public abstract class VersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository> : DomainServiceBase<TDomain, TRepository, TIdentifier>, IVersioningSupportedService<TDomain, TIdentifier, TVersion, TRepository>
        where TDomain : DomainEntityBase<TIdentifier>, IVersioningSupportedEntity<TIdentifier, TVersion>
        where TRepository : class, IRepository<TDomain, TIdentifier>
    {
        public VersioningSupportedService(TRepository repository)
            : base(repository)
        {
            OnRemoveValidators = new List<IServiceValidator<TDomain, TIdentifier>>();
            OnSaveValidators = new List<IServiceValidator<TDomain, TIdentifier>>();
        }
        public abstract override TDomain CreateNullObject();

        protected abstract Expression<Func<TDomain, bool>> GetEntityFinderPredicate(TDomain domain);
        protected abstract Expression<Func<TDomain, TVersion>> SortingKeySelector { get; }
        public virtual TDomain GetLatestVersion(TDomain domain)
        {
            TDomain entity = GetLatestVersionQuery().Where(GetEntityFinderPredicate(domain)).OrderByDescending(SortingKeySelector).FirstOrDefault();
            if (entity != null)
            {
                return entity;
            }
            else
                return null;
        }
        protected override void SaveOperation(TDomain domain)
        {
            SaveWithVersion(domain);
        }

        protected virtual void SaveWithVersion(TDomain domain)
        {
            TDomain latestVersion = GetLatestVersion(domain);
            CustomOperation(domain, latestVersion);
            if (latestVersion != null)
            {
                int result = domain.CompareTo(latestVersion);
                if (result < 0)
                {
                    CustomPersistanceOperation(domain, latestVersion);
                }
                else if (result > 0)
                {
                    domain.IncreaseVersion(latestVersion.Version);
                    Repository.Add(domain);
                }
                else
                    OnCurrentAndOriginalIsIdentical(domain, latestVersion);
            }
            else
            {
                domain.IncreaseVersion(GetDefaultVersion());
                Repository.Add(domain);
            }
        }

        protected virtual void OnCurrentAndOriginalIsIdentical(TDomain currentVersion, TDomain originalVersion)
        {
        }

        protected virtual TVersion GetDefaultVersion()
        {
            return default(TVersion);
        }

        protected virtual IQueryable<TDomain> GetLatestVersionQuery()
        {
            return Repository.Set();
        }

        protected virtual void CustomPersistanceOperation(TDomain domain, TDomain latestVersion)
        {

        }

        protected virtual void CustomOperation(TDomain domain, TDomain latestVersion)
        {

        }
    }
}
