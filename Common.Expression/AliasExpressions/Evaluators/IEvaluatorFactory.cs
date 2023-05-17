using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Expression.AliasExpressions.Evaluators
{
    public interface IEvaluatorFactory
    {
        IEvaluator GetEvaluator(Type keyType, string expression);
        IEvaluator GetEvaluator(Type keyType, IExpression expression);
    }
}
