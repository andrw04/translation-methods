using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Statements;

public class VarStatement : Statement
{
    public List<(Token, Token)> Variables;

    public VarStatement(List<(Token, Token)> variables)
    {
        Variables = variables;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
