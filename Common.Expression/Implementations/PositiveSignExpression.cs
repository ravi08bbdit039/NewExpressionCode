﻿using Common.Expression.Abstractions;
using Common.Expression.Visitors.Abstractions;

namespace Common.Expression.Implementations
{
    public class PositiveSignExpression : UnaryExpression
    {
        public PositiveSignExpression(IExpression operand)
            : base(ExpressionType.Numeric, operand)
        {
            EnsureTypeSupported(operand, ExpressionType.Numeric, $"PositiveSignExpression operand should support {ExpressionType.Numeric } type. Type supplied: {operand.ResultType}");
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"+({Operand})";
        }
    }
}
