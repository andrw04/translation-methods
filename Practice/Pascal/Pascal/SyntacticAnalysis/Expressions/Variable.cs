using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Variable : Expression
{
    public Token Name;

    public Variable(Token name)
    {
        Name = name;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
