using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Super : Expression
{
    public Token Keyword;
    public Token Method;

    public Super(Token keyword, Token method)
    {
        Keyword = keyword;
        Method = method;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
