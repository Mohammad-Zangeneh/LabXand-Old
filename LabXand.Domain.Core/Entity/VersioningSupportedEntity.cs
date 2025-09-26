using System;

namespace LabXand.DomainLayer.Core
{
    [Serializable]
    public abstract class VersioningSupportedEntity<TIdentifier, TVersion> : DomainEntityBase<TIdentifier>, IVersioningSupportedEntity<TIdentifier, TVersion>, IComparable
    {

        public abstract TVersion Version { get; }

        public abstract void IncreaseVersion(TVersion currentVersion);

        public abstract int CompareTo(object obj);
    }
}
