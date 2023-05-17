using Common.Expression.AliasExpressions.Abstractions;

namespace Common.Expression.AliasExpressions.Evaluators
{
    public interface IEvaluator { }
    public interface IEvaluator<TK, TV> : IEvaluator
    {
        TV Evaluate(IValueProvider<TK, TV> provider);
    }
}
