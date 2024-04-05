using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions;

public class Call : Expression
{
    public Expression Callen;
    public Token Paren;
    public List<Expression> Arguments;

    public Call(Expression callen, Token paren, List<Expression> arguments)
    {
        Callen = callen;
        Paren = paren;
        Arguments = arguments;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
