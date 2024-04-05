using System.Globalization;

namespace Pascal.LexicalAnalysis;

public class Scanner
{
    private string _source;
    private List<Token> _tokens = new List<Token>();
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;
    private int _column = 1;

    private static Dictionary<string, TokenType> _keyWords = new Dictionary<string, TokenType>()
    {
        { "and", TokenType.AND },
        { "array", TokenType.ARRAY },
        { "asm", TokenType.ASM },
        { "begin", TokenType.BEGIN },
        { "break", TokenType.BREAK },
        { "boolean", TokenType.BOOLEAN },
        { "case", TokenType.CASE },
        { "const", TokenType.CONST },
        { "constructor", TokenType.CONSTRUCTOR },
        { "continue", TokenType.CONTINUE },
        { "destructor", TokenType.DESTRUCTOR },
        { "div", TokenType.DIV },
        { "do", TokenType.DO },
        { "downto", TokenType.DOWNTO },
        { "else", TokenType.ELSE },
        { "end", TokenType.END },
        { "file", TokenType.FILE },
        { "for", TokenType.FOR },
        { "function", TokenType.FUNCTION },
        { "goto", TokenType.GOTO },
        { "if", TokenType.IF },
        { "implementation", TokenType.IMPLEMENTATION },
        { "in", TokenType.IN },
        { "inline", TokenType.INLINE },
        { "interface", TokenType.INTERFACE },
        { "label", TokenType.LABEL },
        { "mod", TokenType.MOD },
        { "nil", TokenType.NIL },
        { "object", TokenType.OBJECT },
        { "of", TokenType.OF },
        { "operator", TokenType.OPERATOR },
        { "or", TokenType.OR },
        { "not", TokenType.NOT },
        { "packed", TokenType.PACKED },
        { "procedure", TokenType.PROCEDURE },
        { "program", TokenType.PROGRAM },
        { "record", TokenType.RECORD },
        { "repeat", TokenType.REPEAT },
        { "set", TokenType.SET },
        { "SHL", TokenType.SHL },
        { "SHR", TokenType.SHR },
        { "string", TokenType.STRING },
        { "then", TokenType.THEN },
        { "to", TokenType.TO },
        { "type", TokenType.TYPE },
        { "unit", TokenType.UNIT },
        { "until", TokenType.UNTIL },
        { "uses", TokenType.USES },
        { "var", TokenType.VAR },
        { "while", TokenType.WHILE },
        { "with", TokenType.WITH },
        { "xor", TokenType.XOR },
        { "as", TokenType.AS },
        { "class", TokenType.CLASS },
        { "constref", TokenType.CONSTREF },
        { "dispose", TokenType.DISPOSE },
        { "except", TokenType.EXCEPT },
        { "exit", TokenType.EXIT },
        { "exports", TokenType.EXPORTS },
        { "finalization", TokenType.FINALIZATION },
        { "finally", TokenType.FINALLY },
        { "inherited", TokenType.INHERITED },
        { "is", TokenType.IS },
        { "library", TokenType.LIBRARY },
        { "new", TokenType.NEW },
        { "on", TokenType.ON },
        { "out", TokenType.OUT },
        { "property", TokenType.PROPERTY },
        { "raise", TokenType.RAISE },
        { "self", TokenType.SELF },
        { "threadvar", TokenType.THREADVAR },
        { "try", TokenType.TRY },
    };

    public Scanner(string source)
    {
        _source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _column += _current - _start;
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, _line, 1));

        return _tokens;
    }

    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }

    private void ScanToken()
    {
        char c = Advance();

        switch (c)
        {
            // Single-character tokens
            case '(':
                AddToken(TokenType.LEFT_PARENTHESE);
                break;
            case ')':
                AddToken(TokenType.RIGHT_PARENTHESE);
                break;
            case '[':
                AddToken(TokenType.LEFT_ANGLE_BRACKET);
                break;
            case ']':
                AddToken(TokenType.RIGHT_ANGLE_BRACKET);
                break;
            case ',':
                AddToken(TokenType.COMMA);
                break;
            case '.':
                AddToken(TokenType.DOT);
                break;
            case '-':
                AddToken(TokenType.MINUS);
                break;
            case '+':
                AddToken(TokenType.PLUS);
                break;
            case ':':
                AddToken(Match('=') ? TokenType.ASSIGN : TokenType.COLON);
                break;
            case ';':
                AddToken(TokenType.SEMICOLON);
                break;
            case '*':
                AddToken(TokenType.STAR);
                break;
            case '=':
                AddToken(TokenType.EQUAL);
                break;
            // One- or two-character tokens
            case '<':
                if (Match('='))
                    AddToken(TokenType.LESS_EQUAL);
                else if (Match('>'))
                    AddToken(TokenType.NOT_EQUAL);
                else if (Match('<'))
                    AddToken(TokenType.SHIFT_LEFT);
                else
                    AddToken(TokenType.LESS);
                break;
            case '>':
                if (Match('='))
                    AddToken(TokenType.GREATER_EQUAL);
                else if (Match('>'))
                    AddToken(TokenType.SHIFT_RIGHT);
                else
                    AddToken(TokenType.GREATER);
                break;
            case '/':
                if (Match('/')) // if comment
                    while (Peek() != '\n' && !IsAtEnd())
                        Advance();
                else
                    AddToken(TokenType.SLASH);
                break;
            case '{': // multiline comment
                while (Peek() != '}' && !IsAtEnd())
                    Advance();
                if (IsAtEnd())
                    Pascal.Error(_line, _column, "Unterminated multiline comment");
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                _column = 0;
                _line++;
                break;
            case '"':
            case '\'':
                String(c);
                break;
            default:
                if (IsDigit(c))
                    Number();
                else if (IsAlpha(c) || c == '_')
                    Identifier();
                else
                    Pascal.Error(_line, _column, "Unexpected character");
                break;
        }
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek()))
            Advance();

        string text = _source.Substring(_start, _current - _start);

        TokenType type = TokenType.IDENTIFIER;

        if (_keyWords.ContainsKey(text))
        {
            type = _keyWords[text];
        }

        AddToken(type);
    }

    private bool IsDigit(char symbol) => symbol >= '0' && symbol <= '9';

    private bool IsAlpha(char symbol) =>
        symbol >= 'a' && symbol <= 'z' ||
        symbol >= 'A' && symbol <= 'Z' ||
        symbol == '_';

    private bool IsAlphaNumeric(char symbol)
    {
        return IsAlpha(symbol) || IsDigit(symbol);
    }

    private void Number()
    {
        while (IsDigit(Peek()))
        {
            Advance();
        }

        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();

            while (IsDigit(Peek()))
            {
                Advance();
            }

            if (Peek() == '.')
                Pascal.Error(_line, _column, "Unexpected character");
            return;
        }

        if (IsAlpha(Peek()))
        {
            Pascal.Error(_line, _column, "Unexpected character");
            return;
        }

        AddToken(TokenType.NUMBER_LITERAL,
            double.Parse(_source.Substring(_start, _current - _start), CultureInfo.InvariantCulture));
    }

    private void String(char symbol)
    {
        while (Peek() != symbol && !IsAtEnd())
        {
            if (Peek() == '\n')
                _line++;

            Advance();
        }

        if (IsAtEnd())
        {
            Pascal.Error(_line, _column, "Unterminated string");
            return;
        }

        Advance();

        string value = _source.Substring(_start + 1, _current - _start - 2);
        AddToken(TokenType.STRING_LITERAL, value);
    }

    private bool Match(char expected)
    {
        if (IsAtEnd())
            return false;

        if (_source[_current] != expected)
            return false;

        _current++;

        return true;
    }

    private char Peek()
    {
        if (IsAtEnd())
            return '\0';
        return _source[_current];
    }

    private char PeekNext()
    {
        if (_current + 1 >= _source.Length)
            return '\0';

        return _source[_current + 1];
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private void AddToken(TokenType type) => AddToken(type, null);

    private void AddToken(TokenType type, object? literal)
    {
        string text = _source.Substring(_start, _current - _start);
        _tokens.Add(new Token(type, text, literal, _line, _column));
    }
}
