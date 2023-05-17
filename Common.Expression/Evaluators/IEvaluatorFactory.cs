using Common.Expression.Visitors.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Expression.Evaluators
{
    public interface IEvaluatorFactory
    {
        void AddNameResolver(INameResolver nameResolver);
        IEvaluator GetEvaluator(Type type, string expression);
        IEvaluator GetEvaluator(Type type, IExpression expression);
    }
}
