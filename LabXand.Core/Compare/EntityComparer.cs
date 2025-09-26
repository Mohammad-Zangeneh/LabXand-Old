using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.Core
{
    public abstract class EntityComparer<T> : IEntityComparer<T>
    {
        public EntityComparer()
        {
            Init();
        }
        protected Func<object, object, bool> DefaultEqualityEvaluator
        {
            get
            {
                return EqualityEvaluator;
            }
        }
        bool EqualityEvaluator(object current, object other)
        {
            if (current != null)
                return current.Equals(other);
            else if (other == null)
                return true;
            else
                return false;

        }
        protected abstract void Init();

        public List<Tuple<Func<T, dynamic>, Func<object, object, bool>>> PropertySpecifications { get; set; }

        public virtual int Compare(T current, T other)
        {
            if (PropertySpecifications != null)
            {
                bool result = false;
                foreach (var item in PropertySpecifications)
                {
                    dynamic currentValue = null;
                    if (current != null)
                        currentValue = item.Item1.Invoke(current);
                    dynamic otherValue = null;
                    if (other != null)
                        otherValue = item.Item1.Invoke(other);
                    result = item.Item2.Invoke(currentValue, otherValue);
                    if (!result)
                        break;
                }
                return result ? 0 : 1;
            }
            return 0;
        }
    }
}
