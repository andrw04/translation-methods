using Pascal.SyntacticAnalysis.Expressions;

namespace Pascal.SyntacticAnalysis.Statements;

public class ExpressionStatement : Statement
{
    public Expression Expression;

    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
