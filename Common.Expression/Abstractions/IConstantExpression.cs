namespace Common.Expression.Abstractions
{
    public interface IConstantExpression : IExpression
    {
        object Value { get; }
    }
}
