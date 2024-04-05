using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Logical : Expression
{
    public Expression Left;
    public Token Operator;
    public Expression Right;

    public Logical(Expression left, Token op, Expression right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
