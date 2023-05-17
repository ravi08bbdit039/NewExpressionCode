using Common.Expression.Implementations;
using Common.Expression.Visitors.Abstractions;
using System;
using MsExpression=System.Linq.Expressions;
using System.Reflection;

namespace Common.Expression.Visitors.Implementations
{
    public class PredicateVisitor<T> : VisitorBase, IResultProvider<Func<T, bool>> where T : class
    {
        public override MsExpression.ParameterExpression Parameter { get; } = MsExpression.Expression.Parameter(typeof(T), MemberExpression.DefaultVariableName);
        public readonly INameResolverContainer _nameResolverContainer;

        public PredicateVisitor(INameResolverContainer nameResolverContainer)
        {
            _nameResolverContainer = nameResolverContainer ?? throw new ArgumentNullException(nameof(nameResolverContainer));
        }

        public PredicateVisitor() : this(new NameResolverContainer()) { }

        public Func<T, bool> Get()
        {
            return MsExpression.Expression.Lambda<Func<T, bool>>(Current, Parameter).Compile();
        }

        protected internal override MsExpression.Expression VisitMemberExpression(MemberExpression expression)
        {
            MsExpression.Expression current = Parameter;
            var members = expression.Name.Split('.');
            foreach (var member in members)
            {
                var nameResolver = _nameResolverContainer.Get(current.Type);
                var info = nameResolver.Get(member);
                if (info == null)
                {
                    throw new ArgumentException($"Member '{member}' is not defined in type: {current.Type}");
                }
                if (info is PropertyInfo propertyInfo)
                {
                    current = MsExpression.Expression.Property(current, propertyInfo);
                }
                else
                {
                    current = MsExpression.Expression.Field(current, (FieldInfo)info);
                }
            }
            return current;
        }
    }
}
