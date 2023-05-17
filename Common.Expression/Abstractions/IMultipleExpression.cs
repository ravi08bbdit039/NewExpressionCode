using System.Collections.Generic;

namespace Common.Expression.Abstractions
{
    public interface IMultipleExpression
    {
        IReadOnlyCollection<IExpression> Operands { get; }
    }
}
