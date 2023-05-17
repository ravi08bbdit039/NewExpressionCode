using Common.Expression.Abstractions;
using Common.Expression.Visitors.Abstractions;

namespace Common.Expression.Implementations
{
    public class GreaterThanOrEqualExpression : ComparisonExpression
    {
        public GreaterThanOrEqualExpression(IExpression leftOperand, IExpression rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override string GetExpressionName() => ">=";
    }
}
