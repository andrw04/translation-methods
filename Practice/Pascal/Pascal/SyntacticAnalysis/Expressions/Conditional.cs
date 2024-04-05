namespace Pascal.SyntacticAnalysis.Expressions;

public class Conditional : Expression
{
    public Expression Expression;
    public Expression ThenBranch;
    public Expression ElseBranch;

    public Conditional(Expression expression, Expression thenBranch, Expression elseBranch)
    {
        Expression = expression;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
