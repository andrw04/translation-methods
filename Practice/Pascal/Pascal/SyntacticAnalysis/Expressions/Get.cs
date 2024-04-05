using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Get : Expression
{
    public Expression Target;
    public Token Name;

    public Get(Expression target, Token name)
    {
        Target = target;
        Name = name;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
