using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class This : Expression
{
    public Token Keyword;

    public This(Token keyword)
    {
        Keyword = keyword;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
