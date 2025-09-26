using LabXand.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Extensions
{
    public class LinqExpressionHelper
    {
        public static Expression<Func<TEntity, bool>> CreateContainsMethod<TEntity ,T>(T identifiers, string propertyName , ParameterExpression parameter)
        {
            return Expression.Lambda<Func<TEntity, bool>>(CreateCallMethodExpression(identifiers, "Contains", ExpressionHelper.GetMemberExpression<TEntity>(parameter, propertyName)), new[] { parameter });
        }

        public static Expression CreateCallMethodExpression<T>(object source, string methodName, object inputValue, ParameterExpression parameter)
        {
            if (parameter == null)
            {
                parameter = Expression.Parameter(typeof(T));
            }
            MemberExpression memberExpression = ExpressionHelper.GetMemberExpression<T>(parameter, inputValue.ToString());
            return CreateCallMethodExpression(source, methodName, memberExpression);
        }

        public static Expression CreateCallMethodExpression(object source, string methodName, MemberExpression memberExpression)
        {
            ConstantExpression sourceConstantExpression = Expression.Constant(source, source.GetType());
            MethodInfo targetMethod = source.GetType().GetMethod(methodName);

            if (targetMethod != null)
            {
                return Expression.Call(sourceConstantExpression, targetMethod, memberExpression);
            }
            throw new Exception(string.Format("{0} method not defined in {1} type.", methodName, source.GetType().Name));
        }
    }
}
