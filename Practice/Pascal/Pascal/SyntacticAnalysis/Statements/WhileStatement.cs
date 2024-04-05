using Pascal.SyntacticAnalysis.Expressions;

namespace Pascal.SyntacticAnalysis.Statements;

public class WhileStatement : Statement
{
    public Expression Condition;
    public Statement Body;

    public WhileStatement(Expression condition, Statement body)
    {
        Condition = condition;
        Body = body;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
