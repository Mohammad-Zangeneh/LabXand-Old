using LabXand.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LabXand.Core
{
    public partial class Guard
    {

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty<T, TProperty>(Expression<Func<T>> propertySelector, Func<TProperty> value)
        {
            TProperty propertyValue = value();
            if (propertyValue == null || propertyValue.Equals(default(TProperty)))
            {
                string argumentName = ExpressionHelper.GetPropertyName(propertySelector);
                throw Error.Argument(argumentName, "'{0}' property of '{1}' instance cannot be an empty or null.", argumentName, typeof(T).Name);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentInRange<T, TProperty>(Expression<Func<T>> propertySelector, Func<TProperty> value, TProperty min, TProperty max) where TProperty : struct, IComparable<TProperty>
        {
            TProperty propertyValue = value();
            if (propertyValue.CompareTo(min) < 0 || propertyValue.CompareTo(max) > 0)
            {
                string propertyName = ExpressionHelper.GetPropertyName(propertySelector);
                throw Error.ArgumentOutOfRange(propertyName, "The '{0}' must be between '{1}' and '{2}'.", propertyName, min, max);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotOutOfLength<T>(Expression<Func<T>> propertySelector, Func<string> value, int maxLength)
        {
            string propertyValue = value();
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                if (propertyValue.Trim().Length > maxLength)
                {
                    string propertyName = ExpressionHelper.GetPropertyName(propertySelector);
                    throw Error.Argument(propertyName, "'{0}' cannot be more than {1} characters long.", propertyName, maxLength);
                }
            }
        }

        [DebuggerStepThrough]
        public static void PropertyNotNullOrEmpty<T, TProperty>(T instance, Expression<Func<T, TProperty>> propertySelector)
        {
            ArgumentNotNull(instance, "instance");
            TProperty propertyValue = propertySelector.Compile().Invoke(instance);
            if (propertyValue == null || propertyValue.Equals(default(T)))
            {
                string argName = ExpressionHelper.GetPropertyName(propertySelector);
                throw Error.Argument(argName, "'{0}' property of '{1}' instance cannot be an empty or null.", argName, typeof(T).Name);
            }
        }

        [DebuggerStepThrough]
        public static void PropertyInRange<T, TProperty>(T instance, Expression<Func<T, TProperty>> propertySelector, TProperty min, TProperty max) where TProperty : struct, IComparable<TProperty>
        {
            ArgumentNotNull(() => instance);
            TProperty propertyValue = propertySelector.Compile().Invoke(instance);
            if (propertyValue.CompareTo(min) < 0 || propertyValue.CompareTo(max) > 0)
            {
                string propertyName = ExpressionHelper.GetPropertyName(propertySelector);
                throw Error.ArgumentOutOfRange(propertyName, "The '{0}' must be between '{1}' and '{2}'.", propertyName, min, max);
            }
        }

        [DebuggerStepThrough]
        public static void PropertyNotOutOfLength<T>(T instance, Expression<Func<T, string>> propertySelector, int maxLength)
        {
            ArgumentNotNull(() => instance);
            string propertyValue = propertySelector.Compile().Invoke(instance);
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                if (propertyValue.Trim().Length > maxLength)
                {
                    string propertyName = ExpressionHelper.GetPropertyName(propertySelector);
                    throw Error.Argument(propertyName, "'{0}' cannot be more than {1} characters long.", propertyName, maxLength);
                }
            }
        }
    }
}
