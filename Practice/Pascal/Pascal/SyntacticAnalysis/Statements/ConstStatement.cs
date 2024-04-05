using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Statements;

public class ConstStatement : Statement
{
    public Dictionary<Token, Token> Variables;
    
    public ConstStatement(Dictionary<Token, Token> variables)
    {
        Variables = variables;
    }
    
    public override T Accept<T>(IVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}