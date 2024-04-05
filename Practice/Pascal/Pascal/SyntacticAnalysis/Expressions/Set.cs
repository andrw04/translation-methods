using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Set : Expression
{
    public Expression Target;
    public Token Name;
    public Expression Value;

    public Set(Expression target, Token name, Expression value)
    {
        Target = target;
        Name = name;
        Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
