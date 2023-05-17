namespace Common.Expression.Abstractions
{
    public interface IUnaryExpression
    {
        IExpression Operand { get; }
    }
}
