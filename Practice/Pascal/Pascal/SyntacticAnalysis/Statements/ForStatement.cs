using Pascal.LexicalAnalysis;
using Pascal.SyntacticAnalysis.Expressions;

namespace Pascal.SyntacticAnalysis.Statements;

public class ForStatement : Statement
{
    public Expression Initialization;
    public Expression FinalValue;
    public Token Direction;
    public Statement Body;

    public ForStatement(Expression init, Expression final, Token dir, Statement body)
    {
        Initialization = init;
        FinalValue = final;
        Direction = dir;
        Body = body;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
