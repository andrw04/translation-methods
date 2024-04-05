namespace Pascal.SyntacticAnalysis.Statements;

public class TypeStatement : Statement
{

    public TypeStatement()
    {
        
    }
    
    public override T Accept<T>(IVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}