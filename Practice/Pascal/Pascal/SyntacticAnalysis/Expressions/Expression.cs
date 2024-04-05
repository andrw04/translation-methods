namespace Pascal.SyntacticAnalysis.Expressions;

public abstract class Expression
{
    public abstract T Accept<T>(IVisitor<T> visitor); // базовый метод для обхода дерева
}
