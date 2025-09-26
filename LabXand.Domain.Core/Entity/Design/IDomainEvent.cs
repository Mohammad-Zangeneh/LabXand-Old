using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.DomainLayer.Core
{
    public interface IDomainEvent
    {
        string Name { get; }
        string GetDescription();
    }

    public interface IDomainEvent<TDomain, TIdentifier> : IDomainEvent
        where TDomain : DomainEntityBase<TIdentifier>
    {
        TDomain AffectedDomain { get; set; }
    }

    public class DomainWasCreatedEvent<TDomain, TIdentifier> : IDomainEvent<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {

        public TDomain AffectedDomain { get; set; }

        public string Name
        {
            get
            {
                return "Domain was created.";
            }
        }

        public string GetDescription()
        {
            if (AffectedDomain != null)
                return string.Format("The '{0}' from type '{1}' was created.", AffectedDomain.EntityDescriptor(), AffectedDomain.GetType().Name);
            return string.Empty;
        }
    }

    public class DomainWasModifiedEvent<TDomain, TIdentifier> : IDomainEvent<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {

        public TDomain AffectedDomain { get; set; }

        public string Name
        {
            get
            {
                return "Domain was modified.";
            }
        }

        public string GetDescription()
        {
            if (AffectedDomain != null)
                return string.Format("The '{0}' from type '{1}' was modified.", AffectedDomain.EntityDescriptor(), AffectedDomain.GetType().Name);
            return string.Empty;
        }
    }

    public class DomainWasDeletedEvent<TDomain, TIdentifier> : IDomainEvent<TDomain, TIdentifier>
        where TDomain : DomainEntityBase<TIdentifier>
    {

        public TDomain AffectedDomain { get; set; }

        public string Name
        {
            get
            {
                return "Domain was deleted.";
            }
        }

        public string GetDescription()
        {
            if (AffectedDomain != null)
                return string.Format("The '{0}' from type '{1}' was deleted.", AffectedDomain.EntityDescriptor(), AffectedDomain.GetType().Name);
            return string.Empty;
        }
    }
}
