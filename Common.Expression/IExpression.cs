using Common.Expression.Visitors.Abstractions;

namespace Common.Expression
{
    public interface IExpression
    {
        ExpressionType ResultType { get; }
        void Accept(IVisitor visitor);
    }
}
