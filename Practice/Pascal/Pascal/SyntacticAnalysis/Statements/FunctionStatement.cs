using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Statements;

public class FunctionStatement : Statement
{
    public Token Name;
    public Token ReturnType;
    public List<(Token, Token)> Parameters;
    public BlockStatement Body;
    
    public FunctionStatement(Token name, Token returnType, List<(Token, Token)> parameters, BlockStatement body)
    {
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
