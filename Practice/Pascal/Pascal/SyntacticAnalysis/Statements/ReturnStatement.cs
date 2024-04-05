using Pascal.LexicalAnalysis;
using Pascal.SyntacticAnalysis.Expressions;

namespace Pascal.SyntacticAnalysis.Statements;

public class ReturnStatement : Statement
{
    public Token Keyword;
    public Expression Value;

    public ReturnStatement(Token keyword, Expression value)
    {
        Keyword = keyword;
        Value = value;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
