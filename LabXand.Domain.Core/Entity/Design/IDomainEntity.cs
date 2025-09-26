using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface IDomainEntity
    {
        object Id { get; }
        string EntityDescriptor();
        bool IsValid();
    }
    public interface IDomainEntity<TIdentifier> : IEquatable<IDomainEntity<TIdentifier>>, IDomainEntity
    {
        void SetId(TIdentifier id);
        new TIdentifier Id { get; }        
        List<IDomainEvent> Events { get; }
    }
}
