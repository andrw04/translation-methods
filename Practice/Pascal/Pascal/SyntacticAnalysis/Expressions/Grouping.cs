namespace Pascal.SyntacticAnalysis.Expressions
{
    public class Grouping : Expression
    {
        public Expression Expression;

        public Grouping(Expression expression)
        {
            Expression = expression;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
