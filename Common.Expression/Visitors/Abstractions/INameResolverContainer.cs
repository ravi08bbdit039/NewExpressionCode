using System;

namespace Common.Expression.Visitors.Abstractions
{
    public interface INameResolverContainer
    {
        INameResolver Get(Type type);
    }
}
