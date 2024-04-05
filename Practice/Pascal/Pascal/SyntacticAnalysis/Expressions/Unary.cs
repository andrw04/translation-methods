using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions
{
    public class Unary : Expression
    {
        public Expression Right;
        public Token Operator;
        public Unary(Token op, Expression r)
        {
            Operator = op;
            Right = r;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
