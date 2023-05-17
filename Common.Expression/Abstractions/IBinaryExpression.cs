namespace Common.Expression.Abstractions
{
    public interface IBinaryExpression
    {
        IExpression LeftOperand { get; }
        IExpression RightOperand { get; }
    }
}
