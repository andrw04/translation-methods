using Pascal.SyntacticAnalysis.Expressions;
using Pascal.SyntacticAnalysis.Statements;
using System.Data;
using System.Text;

namespace Pascal.SyntacticAnalysis;

public class Printer : IVisitor<string>
{
    public int NestedDepth { get; set; } = 0;

    public string Print(Expression expression)
    {
        return expression.Accept(this);
    }

    public string Print(Statement statement)
    {
        return statement.Accept(this);
    }

    public string Visit(Assign expression)
    {
        StringBuilder sb = new StringBuilder();
        
        sb.Append($"Assignment");

        var depth = IncDepth();
        sb.Append($"\n{depth}Variable ({expression.Name.Lexeme})");
        sb.Append($"\n{depth}Expression");

        depth = IncDepth();
        sb.Append($"\n{depth}{expression.Value.Accept(this)}");
        DecDepth();

        DecDepth();

        return sb.ToString();
    }

    public string Visit(Binary expression)
    {
        StringBuilder sb = new StringBuilder();

        var depth = Depth();
        sb.Append($"BinaryOperation ({expression.Operator.Lexeme})");

        depth = IncDepth();
        sb.Append($"\n{depth}Operand");
        
        depth = IncDepth();
        sb.Append($"\n{depth}{expression.Left.Accept(this)}");
        depth = DecDepth();
        
        sb.Append($"\n{depth}Operand");
        
        depth = IncDepth();
        sb.Append($"\n{depth}{expression.Right.Accept(this)}");
        DecDepth();

        DecDepth();

        return sb.ToString();
    }

    public string Visit(Call expression)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"FunctionCall ({expression.Callen.Accept(this)})");
        var depth = IncDepth();
        sb.Append($"\n{depth}Arguments");

        depth = IncDepth();
        foreach (var arg in expression.Arguments)
        {
            sb.Append($"\n{depth}{arg.Accept(this)}");
        }
        DecDepth();

        DecDepth();

        return sb.ToString();
    }

    public string Visit(Get expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(Grouping expression)
    {
        return Parenthesize("group", expression.Expression);
    }

    public string Visit(Literal expression)
    {
        return $"'{expression.Value.ToString() ?? "nil"}'";
    }

    public string Visit(Logical expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(Set expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(Super expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(This expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(Unary expression)
    {
        return Parenthesize(expression.Operator.Lexeme, expression.Right);
    }

    public string Visit(Prefix expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(Postfix expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(Conditional expression)
    {
        throw new NotImplementedException();
    }

    public string Visit(Variable expression)
    {
        return expression.Name.Lexeme;
    }

    public string Visit(BlockStatement statement)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"CodeBlock:");

        var depth = IncDepth();
        foreach (var s in statement.Statements)
        {
            sb.Append($"\n{depth}{s.Accept(this)}");
        }

        DecDepth();

        return sb.ToString();
    }

    public string Visit(ClassStatement statement)
    {
        throw new NotImplementedException();
    }

    public string Visit(ExpressionStatement statement)
    {
        return statement.Expression.Accept(this);
    }

    public string Visit(FunctionStatement statement)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"FunctionDeclaration");

        var depth = IncDepth();
        sb.Append($"\n{depth}ReturnType ({statement.ReturnType.Lexeme})");
        sb.Append($"\n{depth}FunctionName ({statement.Name.Lexeme})");
        sb.Append($"\n{depth}Parameters");

        depth = IncDepth();
        foreach (var param in statement.Parameters)
        {
            sb.Append($"\n{depth}Parameter ({param.Item1.Lexeme}: {param.Item2.Lexeme})");
        }
        depth = DecDepth();
        
        sb.Append($"\n{depth}FunctionBody");

        depth = IncDepth();
        foreach (var s in statement.Body.Statements)
        {
            sb.Append($"\n{depth}{s.Accept(this)}");
        }
        DecDepth();

        DecDepth();

        return sb.ToString();
    }

    public string Visit(IfStatement statement)
    {
        StringBuilder sb = new StringBuilder();
        
        sb.Append($"IfStatement");

        var depth = IncDepth();
        sb.Append($"\n{depth}Condition");

        depth = IncDepth();
        sb.Append($"\n{depth}{statement.Condition.Accept(this)}");
        depth = DecDepth();

        sb.Append($"\n{depth}TrueBranch");

        depth = IncDepth();
        sb.Append($"\n{depth}{statement.ThenBranch.Accept(this)}");
        depth = DecDepth();

        sb.Append($"\n{depth}FalseBranch");

        depth = IncDepth();
        sb.Append($"\n{depth}{statement.ElseBranch.Accept(this)}");
        DecDepth();

        DecDepth();

        return sb.ToString();
    }

    public string Visit(ProcedureStatement statement)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"ProcedureDeclaration");

        var depth = IncDepth();
        sb.Append($"\n{depth}ProcedureName ({statement.Name.Lexeme})");
        sb.Append($"\n{depth}Parameters");

        depth = IncDepth();
        foreach (var param in statement.Parameters)
        {
            sb.Append($"\n{depth}Parameter ({param.Item1.Lexeme}: {param.Item2.Lexeme})");
        }
        depth = DecDepth();
        
        sb.Append($"\n{depth}ProcedureBody");

        depth = IncDepth();
        foreach (var s in statement.Body.Statements)
        {
            sb.Append($"\n{depth}{s.Accept(this)}");
        }
        DecDepth();

        DecDepth();

        return sb.ToString();
    }

    public string Visit(ReturnStatement statement)
    {
        throw new NotImplementedException();
    }

    public string Visit(VarStatement statement)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"VariableBlock:");

        var depth = IncDepth();
        foreach (var v in statement.Variables)
        {
            sb.Append($"\n{depth}VariableDeclaration");
            depth = IncDepth();
            sb.Append($"\n{depth}VariableName ({v.Item1.Lexeme})");
            sb.Append($"\n{depth}VariableType ({v.Item2.Lexeme})");
            DecDepth();
        }

        DecDepth();
        
        return sb.ToString();
    }

    public string Visit(WhileStatement statement)
    {
        throw new NotImplementedException();
    }

    public string Visit(ProgramStatement statement)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Program: {statement.Name.Lexeme}");

        var depth = IncDepth();
        foreach (var s in statement.Body)
        {
            sb.Append($"\n{depth}{s.Accept(this)}");
        }
        DecDepth();


        return sb.ToString();
    }

    public string Visit(ForStatement statement)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"{Depth()}for:\n");
        this.NestedDepth++;
        sb.Append($"initializer: {statement.Initialization.Accept(this)}\n");
        sb.Append($"direction: {statement.Direction.Lexeme}\n");
        sb.Append($"finalValue: {statement.FinalValue.Accept(this)}\n");
        this.NestedDepth++;
        sb.Append($"body:\n");
        sb.Append(statement.Body.Accept(this));
        this.NestedDepth--;
        this.NestedDepth--;

        return sb.ToString();
    }

    private string Parenthesize(string name, params Expression[] expressions)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("(").Append(name);
        foreach (Expression expression in expressions)
        {
            sb.Append(" ");
            sb.Append(expression.Accept(this));
        }
        sb.Append(")");

        return sb.ToString();
    }

    private string IncDepth()
    {
        return new String(' ', this.NestedDepth += 2);
    }

    private string DecDepth()
    {
        return new String(' ', this.NestedDepth -= 2);
    }

    private string Depth()
    {
        return new string(' ', this.NestedDepth);
    }
}
