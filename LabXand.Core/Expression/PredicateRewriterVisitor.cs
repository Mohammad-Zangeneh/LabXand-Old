using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LabXand.Extensions
{
    public class PredicateRewriterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _parameterExpression;

        public PredicateRewriterVisitor(ParameterExpression parameterExpression)
        {
            _parameterExpression = parameterExpression;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterExpression.Type.Equals(node.Type))
                return _parameterExpression;
            return node;
        }
    }

}
