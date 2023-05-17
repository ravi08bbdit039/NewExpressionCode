namespace Common.Expression.Visitors.Abstractions
{
    public interface IResultProvider<T>
    {
        void Reset();
        T Get();
    }
}
