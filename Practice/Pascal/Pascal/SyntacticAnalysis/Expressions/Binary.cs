using Pascal.LexicalAnalysis;

namespace Pascal.SyntacticAnalysis.Expressions
{
    public class Binary : Expression
    {
        public Expression Left;
        public Token Operator;
        public Expression Right;

        public Binary(Expression l, Token op, Expression r)
        {
            Left = l;
            Operator = op;
            Right = r;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
