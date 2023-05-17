using Common.Expression.AliasExpressions.Abstractions;
using Common.Expression.Implementations;
using Common.Expression.Visitors.Abstractions;
using Common.Expression.Visitors.Implementations;
using System;
using System.Reflection;
using MsExpressions = System.Linq.Expressions;

namespace Common.Expression.AliasExpressions.Implementations
{
    public class NumericVisitor<TK> : VisitorBase, IResultProvider<Func<IValueProvider<TK, double>, double>>
    {
        private static readonly MethodInfo ProviderGetMethodInfo = typeof(IValueProvider<TK, double>).GetMethod(nameof(IValueProvider<TK, double>), new Type[] { typeof(TK) });
        public override MsExpressions.ParameterExpression Parameter { get; } = MsExpressions.Expression.Parameter(typeof(IValueProvider<TK, double>), MemberExpression.DefaultVariableName);

        public Func<IValueProvider<TK, double>, double> Get()
        {
            return MsExpressions.Expression.Lambda<Func<IValueProvider<TK, double>, double>>(Current, Parameter).Compile();
        }

        protected internal override MsExpressions.Expression VisitMemberExpression(MemberExpression expression)
        {
            MsExpressions.Expression current = Parameter;
            current = MsExpressions.Expression.Call(Parameter, ProviderGetMethodInfo, MsExpressions.Expression.Constant(expression.Name));
            return current;
        }

    }
}
