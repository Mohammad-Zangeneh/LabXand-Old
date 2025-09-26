using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.DomainLayer.Core
{
    public interface IVersioningSupportedEntity<TIdentifier, TVersion> : IComparable, IDomainEntity<TIdentifier>
    {
        TVersion Version { get; }
        void IncreaseVersion(TVersion currentVersion);
    }
}
