using System;
using System.Reflection;

namespace Common.Expression.Visitors.Abstractions
{
    public interface INameResolver
    {
        Type Type { get; }
        MemberInfo Get(string name);
    }
}
