using System;
using System.Collections.Generic;

namespace LabXand.DomainLayer.Core
{
    [Serializable]
    public abstract class DomainEntityBase<TIdentifier> : IDomainEntity<TIdentifier>, IEquatable<IDomainEntity<TIdentifier>>
    {
        //public DomainEntityBase()
        //{
        //    eventList = new List<IDomainEvent>();
        //    eventList.Add(new DomainWasCreatedEvent<DomainEntityBase<TIdentifier>, TIdentifier>());
        //    eventList.Add(new DomainWasCreatedEvent<DomainEntityBase<TIdentifier>, TIdentifier>());
        //    eventList.Add(new DomainWasCreatedEvent<DomainEntityBase<TIdentifier>, TIdentifier>());
        //}
        //List<IDomainEvent> eventList;
        public virtual List<IDomainEvent> Events
        {
            get
            {
                return new List<IDomainEvent>();
            }
        }
        //{
        //    get
        //    {
        //        return eventList;
        //    }
        //}

        public virtual TIdentifier Id { get; protected set; }

        object IDomainEntity.Id
        {
            get
            {
                return Id;
            }
        }

        public virtual void SetId(TIdentifier id)
        {
            Id = id;
        }

        public virtual string EntityDescriptor()
        {
            return string.Format("{0} with Id {1}", this.GetType().Name, this.Id);
        }

        public virtual bool IsValid()
        {
            return true;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(IDomainEntity<TIdentifier> other)
        {
            if (other == null)
                return false;

            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return this.Id.Equals(((DomainEntityBase<TIdentifier>)obj).Id);
        }

        public static bool operator ==(DomainEntityBase<TIdentifier> lhs, DomainEntityBase<TIdentifier> rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                    return true;

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(DomainEntityBase<TIdentifier> lhs, DomainEntityBase<TIdentifier> rhs)
        {
            return !(lhs == rhs);
        }
    }
}
