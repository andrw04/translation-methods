using Pascal.SyntacticAnalysis.Expressions;

namespace Pascal.SyntacticAnalysis.Statements;

public class IfStatement : Statement
{
    public Expression Condition;
    public Statement ThenBranch;
    public Statement ElseBranch;
    public IfStatement(Expression condition, Statement thenBranch, Statement elseBranch)
    {
        Condition = condition;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
