using Pascal.LexicalAnalysis;
using Pascal.SyntacticAnalysis.Expressions;
using Pascal.SyntacticAnalysis.Statements;

namespace Pascal.SyntacticAnalysis;

public class Parser
{
    private class ParseError : Exception { }

    private List<Token> _tokens;
    private int current = 0;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public Statement? Parse()
    {
        return Program();
    }

    private Statement? Program()
    {
        try
        {
            Consume(TokenType.PROGRAM, "Expected 'program' at start of file.");
            var name = Consume(TokenType.IDENTIFIER, "Expected program name.");
            Consume(TokenType.SEMICOLON, "Expected ';' after " + name);

            List<Statement> statements = new List<Statement>();

            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return new ProgramStatement(name, statements);
        }
        catch (ParseError)
        {
            //Synchronize();
            return null;
        }
    }

    private Statement Declaration()
    {
        if (Match(TokenType.FUNCTION))
            return FunctionDeclaration();
        if (Match(TokenType.PROCEDURE))
            return ProcedureDeclaration();
        if (Match(TokenType.CLASS))
            return ClassDeclaration();
        if (Match(TokenType.CONST))
            return ConstDeclaration();
        if (Match(TokenType.TYPE))
            return TypeDeclaration();
        if (Match(TokenType.VAR))
            return VarDeclaration();
        if (Check(TokenType.BEGIN))
            return BlockDeclaration();

        return Statement();
    }

    private Statement FunctionDeclaration()
    {
        return Function();
    }

    private Statement ProcedureDeclaration()
    {
        return Function(false);
    }

    private Statement ClassDeclaration()
    {
        throw new NotImplementedException();
    }

    private Statement Function(bool hasReturnType = true)
    {
        var getName = () => hasReturnType ? "function" : "procedure";
        
        var name = Consume(TokenType.IDENTIFIER, $"Expected {getName} name.");

        Consume(TokenType.LEFT_PARENTHESE, "Expected '('.");
        var parameters = Parameters();
        Consume(TokenType.RIGHT_PARENTHESE, "Expected ')'.");
        if (hasReturnType)
        {
            Consume(TokenType.COLON, "Expected ':'.");
            Token returnType = Consume(TokenType.IDENTIFIER, "Expected return type.");
            Consume(TokenType.SEMICOLON, "Expected ';'.");
            var block = BlockStatement();
            Consume(TokenType.SEMICOLON, "Expected ';'.");
            return new FunctionStatement(name, returnType, parameters, (block as BlockStatement)!);
        }
        else
        {
            Consume(TokenType.SEMICOLON, "Expected ';'.");
            var block = BlockStatement();
            Consume(TokenType.SEMICOLON, "Expected ';'.");
            return new ProcedureStatement(name, parameters, (block as BlockStatement)!);
        }
    }

    private List<(Token, Token)> Parameters()
    {
        List<Token> parameters = new List<Token>();
        List<(Token, Token)> types = new List<(Token, Token)>();

        while (Check(TokenType.IDENTIFIER))
        {
            parameters.Add(Consume(TokenType.IDENTIFIER, "Expected parameter name."));

            while (Match(TokenType.COMMA))
            {
                parameters.Add(Consume(TokenType.IDENTIFIER, "Expected parameter name."));
            }

            Consume(TokenType.COLON, "Expected ':'.");
            var type = Consume(TokenType.IDENTIFIER, "Expected parameter type.");

            foreach (var p in parameters)
            {
                types.Add((p, type));
            }

            parameters.Clear();

            if (!Match(TokenType.SEMICOLON))
                break;
        }

        return types;
    }

    private Statement ConstDeclaration()
    {
        Dictionary<Token, Token> varValues = new Dictionary<Token, Token>();

        while (Check(TokenType.IDENTIFIER))
        {
            var variable = Consume(TokenType.IDENTIFIER, "Expected variable name.");
            Consume(TokenType.EQUAL, "Expected '='.");

            Token value;
            if (Check(TokenType.NUMBER_LITERAL))
                value = Consume(TokenType.NUMBER_LITERAL, "Expected number literal.");
            else if (Check(TokenType.TRUE_LITERAL))
                value = Consume(TokenType.TRUE_LITERAL, "Expected boolean literal.");
            else if (Check(TokenType.FALSE_LITERAL))
                value = Consume(TokenType.FALSE_LITERAL, "Expected boolean literal.");
            else
                value = Consume(TokenType.STRING_LITERAL, "Expected string literal.");
            
            varValues.Add(variable, value);
            Consume(TokenType.SEMICOLON, "Expected ';'.");
        }

        return new ConstStatement(varValues);
    }

    private Statement TypeDeclaration()
    {
        throw new NotImplementedException();
    }
    
    private Statement VarDeclaration()
    {
        List<Token> variables = new List<Token>();
        List<(Token, Token)> varTypes = new List<(Token, Token)>();

        while (Check(TokenType.IDENTIFIER))
        {
            variables.Add(Consume(TokenType.IDENTIFIER, "Expected variale name."));

            while (Match(TokenType.COMMA))
            {
                variables.Add(Consume(TokenType.IDENTIFIER, "Expected variale name."));
            }
            Consume(TokenType.COLON, "Expected ':'.");
            var type = Consume(TokenType.IDENTIFIER, "Expected variable type.");

            for (int i = 0; i < variables.Count; i++)
            {
                varTypes.Add((variables[i], type));
            }
            
            variables.Clear();
            
            Consume(TokenType.SEMICOLON, "Expected ';'.");
        }

        return new VarStatement(varTypes);
    }

    private Statement BlockDeclaration()
    {
        return BlockStatement('.');
    }

    private Statement Statement()
    {
        if (Match(TokenType.IF))
            return IfStatement();
        if (Match(TokenType.FOR))
            return ForStatement();
        if (Match(TokenType.WHILE))
            return WhileStatement();
        if (Check(TokenType.BEGIN))
            return BlockStatement();

        return ExpressionStatement();
    }

    private Statement IfStatement()
    {
        Expression condition = Expression();
        Consume(TokenType.THEN, "Expected 'then'.");

        Statement thenBranch = Statement();
        Statement elseBrach = null;

        if (Match(TokenType.ELSE))
        {
            elseBrach = Statement();
        }

        return new IfStatement(condition, thenBranch, elseBrach);
    }

    private Statement ForStatement()
    {
        Expression initializer = Expression();
        Token direction;

        if (Check(TokenType.TO))
            direction = Consume(TokenType.TO, "Expected 'to'.");
        else
            direction = Consume(TokenType.DOWNTO, "Expected 'downto'.");

        Expression final = Expression();

        Consume(TokenType.DO, "Expected 'do'.");

        Statement body;

        if (Check(TokenType.BEGIN))
            body = BlockStatement(';');
        else
            body = Declaration();

        return new ForStatement(initializer, final, direction, body);
    }

    private Statement WhileStatement()
    {
        throw new NotImplementedException();
    }

    private Statement BlockStatement(char? ending = null)
    {
        Consume(TokenType.BEGIN, "Expected 'begin'.");

        List<Statement> statements = new List<Statement>();
        while (!Check(TokenType.END) && !IsAtEnd())
        {
            statements.Add(Declaration());
        }

        Consume(TokenType.END, "Expected 'END' after block.");

        if (ending != null && ending == ';')
        {
            Consume(TokenType.SEMICOLON, "Expected ';'.");
        }
        else if (ending != null && ending == '.')
        {
            Consume(TokenType.DOT, "Expected '.'.");
        }
        

        return new BlockStatement(statements);
    }

    private Statement ExpressionStatement()
    {
        Expression expression = Expression();
        Consume(TokenType.SEMICOLON, "Expected ';' after value.");

        return new ExpressionStatement(expression);
    }

    private Expression Expression()
    {
        return Assignment();
    }

    private Expression Assignment()
    {
        Expression expression = Or();

        if (Match(TokenType.ASSIGN, TokenType.EQUAL))
        {
            Token assign = Previous();
            Expression value = Assignment();

            if (expression is Variable v)
            {
                Token name = v.Name;
                return new Assign(name, value);
            }

            Error(assign, "Invalid assignment target.");
        }

        return expression;
    }

    private Expression Or()
    {
        Expression expression = And();

        while (Match(TokenType.OR))
        {
            Token op = Previous();
            Expression right = And();

            expression = new Logical(expression, op, right);
        }

        return expression;
    }

    private Expression And()
    {
        Expression expression = Equality();

        while (Match(TokenType.AND))
        {
            Token op = Previous();
            Expression right = Equality();

            expression = new Logical(expression, op, right);
        }

        return expression;
    }

    private Expression Equality()
    {
        Expression expression = Comparison();

        while (Match(TokenType.NOT_EQUAL, TokenType.EQUAL))
        {
            Token op = Previous();
            Expression right = Comparison();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private Expression Comparison()
    {
        Expression expression = Addition();

        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token op = Previous();
            Expression right = Addition();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private Expression Addition()
    {
        Expression expression = Multiplication();

        while (Match(TokenType.MINUS, TokenType.PLUS))
        {
            Token op = Previous();
            Expression right = Multiplication();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private Expression Multiplication()
    {
        Expression expression = Unary();

        while (Match(TokenType.SLASH, TokenType.STAR))
        {
            Token op = Previous();
            Expression right = Unary();
            expression = new Binary(expression, op, right);
        }

        return expression;
    }

    private Expression Unary()
    {
        if (Match(TokenType.NOT, TokenType.MINUS))
        {
            Token op = Previous();
            Expression right = Unary();
            return new Prefix(op, right);
        }

        return Call();
    }

    private Expression Prefix()
    {
        if (Match(TokenType.NOT, TokenType.MINUS))
        {
            Token op = Previous();
            Expression right = Prefix();
            return new Prefix(op, right);
        }

        return Call();
    }

    private Expression Call()
    {
        Expression expression = Primary();

        while (true)
        {
            if (Match(TokenType.LEFT_PARENTHESE))
            {
                expression = FinishCall(expression);
            }
            else
            {
                break;
            }
        }

        return expression;
    }

    private Expression FinishCall(Expression callee)
    {
        List<Expression> arguments = new List<Expression>();

        if (!Check(TokenType.RIGHT_PARENTHESE))
        {
            do
            {
                if (arguments.Count >= 255)
                    Error(Peek(), "Cannot have more than 255 arguments.");
                arguments.Add(Expression());
            }
            while (Match(TokenType.COMMA));
        }

        Token paren = Consume(TokenType.RIGHT_PARENTHESE, "Exprected ')'.");

        return new Call(callee, paren, arguments);
    }

    private Expression Primary()
    {
        if (Match(TokenType.TRUE_LITERAL))
            return new Literal(true);
        if (Match(TokenType.FALSE_LITERAL))
            return new Literal(false);
        if (Match(TokenType.NIL))
            return new Literal(null);
        if (Match(TokenType.NUMBER_LITERAL, TokenType.STRING_LITERAL))
            return new Literal(Previous().Literal!);
        if (Match(TokenType.IDENTIFIER))
            return new Variable(Previous());

        if (Match(TokenType.LEFT_PARENTHESE))
        {
            Expression expression = Expression();
            Consume(TokenType.RIGHT_PARENTHESE, "Expected ')' after expression.");
            return new Grouping(expression);
        }

        throw Error(Peek(), "Expected expression.");
    }

    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type))
            return Advance();

        throw Error(Peek(), message);
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd())
        {
            return false;
        }

        return Peek().Type == type;
    }

    private Token Advance()
    {
        if (!IsAtEnd())
            current++;
        return Previous();
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Peek()
    {
        return _tokens[current];
    }

    private Token Previous()
    {
        return _tokens[current - 1];
    }

    private ParseError Error(Token token, string message)
    {
        Pascal.Error(token, message);
        return new ParseError();
    }
}
