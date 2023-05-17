using Common.Expression.Abstractions;

namespace Common.Expression.Parsers
{
    public interface IParserConfiguration
    {
        string GetToken(ExpressionKind kind);
        ExpressionKind? GetKind(string value);
        int GetPriority(ExpressionKind kind);
        bool IsFuntion(ExpressionKind kind);
    }
}
