using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LabXand.Core
{
    public interface IConditionalExpressionBuilder
    {
        Expression Get(MemberExpression memberExpression, object value, Type valueType);
    }
}
