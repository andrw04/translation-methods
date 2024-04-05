using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Prefix : Expression
{
    public Token Operator;
    public Expression Right;

    public Prefix(Token op, Expression right)
    {
        Operator = op;
        Right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
