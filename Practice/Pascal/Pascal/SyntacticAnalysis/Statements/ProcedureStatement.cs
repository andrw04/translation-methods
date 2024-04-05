using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Statements;

public class ProcedureStatement : Statement
{
    public Token Name;
    public List<(Token, Token)> Parameters;
    public BlockStatement Body;
    
    public ProcedureStatement(Token name, List<(Token, Token)> parameters, BlockStatement body)
    {
        Name = name;
        Parameters = parameters;
        Body = body;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}