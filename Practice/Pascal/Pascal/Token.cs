namespace Pascal;

public class Token
{
    private TokenType _type;
    private string _lexeme;
    private object? _literal;
    private int _line;
    private int _column;

    public TokenType Type {  get { return _type; } }
    public string Lexeme { get { return _lexeme; } }
    public object? Literal { get { return _literal; } }
    public int Line { get { return _line; } }
    public int Column { get { return _column; } }

    public Token(TokenType type, string lexeme, object? literal, int line, int column)
        : this(type, lexeme, literal, line)
    {
        _column = column;
    }

    public Token(TokenType type, string lexeme, object? literal, int line)
    {
        _type = type;
        _lexeme = lexeme;
        _literal = literal;
        _line = line;
    }

    public override string ToString()
    {
        return $"{_type}\t{_lexeme}\t{_literal}";
    }
}
