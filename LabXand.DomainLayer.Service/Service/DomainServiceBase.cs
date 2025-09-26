using LabXand.DomainLayer.Core;
using LabXand.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer
{
    public abstract class DomainServiceBase<TDomain, TRepository, TIdentifier> : ReadOnlyDomainServiceBase<TDomain, TRepository, TIdentifier>, IDomainServiceWithRepository<TDomain, TRepository, TIdentifier>
        where TRepository : class,IRepository<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {
        public DomainServiceBase(TRepository repository)
            : base(repository)
        {
            OnRemoveValidators = new List<IServiceValidator<TDomain, TIdentifier>>();
            OnSaveValidators = new List<IServiceValidator<TDomain, TIdentifier>>();
        }

        public List<IServiceValidator<TDomain, TIdentifier>> OnSaveValidators { get; protected set; }
        public List<IServiceValidator<TDomain, TIdentifier>> OnRemoveValidators { get; protected set; }

        public virtual TDomain Save(TDomain domain)
        {
            OnBeforeSave(domain);
            CheckValidation(OnSaveValidators, domain);
            SaveOperation(domain);
            OnAfterSave(domain);
            return domain;
        }

        protected virtual void OnBeforeSave(TDomain domain) { }

        protected virtual void OnAfterSave(TDomain domain) { }

        protected virtual void SaveOperation(TDomain domain)
        {
            if (domain.Id.Equals(default(TIdentifier)))
                Repository.Add(domain);
            else
            {
                if (this.Count(ExpressionHelper.CreateEqualCondition<TDomain, TIdentifier>(ExpressionHelper.GetNameOfProperty<TDomain, TIdentifier>(d => d.Id), domain.Id)) > 0)
                    Repository.Edit(domain);
                else
                    Repository.Add(domain);
            }
        }

        protected void CheckValidation(List<IServiceValidator<TDomain, TIdentifier>> validationList, TDomain domain)
        {
            domain.IsValid();
            foreach (var item in validationList)
            {
                if (!item.IsValid(domain))
                    break;
            }
        }

        public virtual bool Remove(TDomain domain)
        {
            CheckValidation(OnRemoveValidators, domain);
            OnBeforeRemove(domain);
            Repository.Remove(domain);
            OnAfterRemove(domain);
            return true;
        }

        protected virtual void OnBeforeRemove(TDomain domain) { }

        protected virtual void OnAfterRemove(TDomain domain) { }
    }

    public interface IKeyGenerator<T>
    {
        T Generate();
    }

    public class DefaultKeyGenerator<T> : IKeyGenerator<T>
    {
        public T Generate()
        {
            return default(T);
        }
    }

    public class GuidKeyGenerator : IKeyGenerator<Guid>
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }


    public enum KeyGenerationTypes
    {
        None,
        NewGuid,
    }
}
