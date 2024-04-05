namespace Pascal.SyntacticAnalysis.Statements;

public class BlockStatement : Statement
{
    public List<Statement> Statements;

    public BlockStatement(List<Statement> statements)
    {
        Statements = statements;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
