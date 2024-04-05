using Pascal.LexicalAnalysis;
using Pascal.SyntacticAnalysis.Expressions;

namespace Pascal.SyntacticAnalysis.Statements;

public class ClassStatement : Statement
{
    public Token Name;
    public Variable Superclass;
    public List<FunctionStatement> Methods;

    public ClassStatement(Token name, Variable superclass, List<FunctionStatement> methods)
    {
        Name = name;
        Superclass = superclass;
        Methods = methods;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
