using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Statements;

public class ProgramStatement : Statement
{
    public Token Name;
    public List<Statement> Body;
    public ProgramStatement(Token name, List<Statement> body)
    {
        Name = name;
        Body = body;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
