using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LabXand.Core
{
    public class NotEqualConditionExpressionBuilder : ConditionalExpressionBuilderBase
    {
        protected override BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.NotEqual(leftExpression, rightExpression);
        }
    }
}
