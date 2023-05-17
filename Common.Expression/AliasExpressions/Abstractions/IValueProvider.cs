namespace Common.Expression.AliasExpressions.Abstractions
{
    public interface IValueProvider<TK, TV>
    {
        TV Get(TK key);
    }
}
