using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LabXand.Core
{
    public class IsNullConditionExpressionBuilder : IConditionalExpressionBuilder
    {
        public Expression Get(MemberExpression memberExpression, object value, Type valueType)
        {
            Type firstdOperandType = ((PropertyInfo)(memberExpression.Member)).PropertyType;
            return Expression.Equal(memberExpression, Expression.Constant(null));            
        }
    }
}
