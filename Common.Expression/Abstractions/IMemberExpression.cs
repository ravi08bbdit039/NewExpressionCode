namespace Common.Expression.Abstractions
{
    public interface IMemberExpression : IExpression
    {
        string Name { get; set; }
    }
}
