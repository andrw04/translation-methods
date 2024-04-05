using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Postfix : Expression
{
    public Token Operator;
    public Expression Left;

    public Postfix(Token op, Expression left)
    {
        Operator = op;
        Left = left;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
