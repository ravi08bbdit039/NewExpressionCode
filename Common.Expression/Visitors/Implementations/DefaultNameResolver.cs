using System;

namespace Common.Expression.Visitors.Implementations
{
    public class DefaultNameResolver : NameResolverBase
    {
        public DefaultNameResolver(Type type) : base(type) { }
    }
}
