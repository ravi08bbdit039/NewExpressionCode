using Common.Expression.AliasExpressions.Abstractions;
using Common.Expression.AliasExpressions.Implementations;
using System;

namespace Common.Expression.AliasExpressions.Evaluators
{
    internal class Evaluator<TK> : IEvaluator<TK, double>
    {
        private readonly Func<IValueProvider<TK, double>, double> _function;

        public Evaluator(IExpression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            var visitor = new NumericVisitor<TK>();
            expression.Accept(visitor);
            _function = visitor.Get();
        }

        public Func<IValueProvider<TK, double>, double> Function => _function;

        public IExpression Expression { get; }

        public double Evaluate(IValueProvider<TK, double> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            return _function(provider);
        }

        public override bool Equals(object obj)
        {
            return obj is Evaluator<TK> other && Expression.Equals(other.Expression);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Expression.GetHashCode();
            }
        }
    }
}