namespace Pascal.SyntacticAnalysis.Expressions
{
    public class Literal : Expression
    {
        public object Value;

        public Literal(object value)
        {

            Value = value;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
