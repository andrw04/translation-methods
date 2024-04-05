using Pascal.SyntacticAnalysis.Expressions;
using Pascal.SyntacticAnalysis.Statements;

namespace Pascal.SyntacticAnalysis;

public interface IVisitor<T>
{
    int NestedDepth { get; set; }
    T Visit(Assign expression);
    T Visit(Binary expression);
    T Visit(Call expression);
    T Visit(Get expression);
    T Visit(Grouping expression);
    T Visit(Literal expression);
    T Visit(Logical expression);
    T Visit(Set expression);
    T Visit(Super expression);
    T Visit(This expression);
    T Visit(Unary expression);
    T Visit(Prefix expression);
    T Visit(Postfix expression);
    T Visit(Conditional expression);
    T Visit(Variable expression);
    T Visit(BlockStatement statement);
    T Visit(ClassStatement statement);
    T Visit(ExpressionStatement statement);
    T Visit(ForStatement statement);
    T Visit(FunctionStatement statement);
    T Visit(IfStatement statement);
    T Visit(ProcedureStatement statement);
    T Visit(ReturnStatement statement);
    T Visit(VarStatement statement);
    T Visit(WhileStatement statement);
    T Visit(ProgramStatement statement);
}
