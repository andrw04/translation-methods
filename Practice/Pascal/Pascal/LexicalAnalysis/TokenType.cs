namespace Pascal.LexicalAnalysis;

public enum TokenType
{
    // Single-character tokens
    LEFT_PARENTHESE,        // (
    RIGHT_PARENTHESE,       // )
    LEFT_ANGLE_BRACKET,     // [
    RIGHT_ANGLE_BRACKET,    // ]
    COMMA,                  // ,
    DOT,                    // .
    MINUS,                  // -
    PLUS,                   // +
    COLON,                  // :
    SEMICOLON,              // ;
    SLASH,                  // /
    STAR,                   // *
    // One- or two-character tokens 
    ASSIGN,                 // :=
    NOT_EQUAL,              // <>
    EQUAL,                  // =
    GREATER,                // >    
    GREATER_EQUAL,          // >=
    LESS,                   // <
    LESS_EQUAL,             // <=
    SHIFT_LEFT,             // <<
    SHIFT_RIGHT,            // >>
    // Literals
    IDENTIFIER,
    STRING_LITERAL,
    NUMBER_LITERAL,
    TRUE_LITERAL,
    FALSE_LITERAL,
    // Keywords
    AND,
    ARRAY,
    ASM,
    BEGIN,
    BREAK,
    BOOLEAN,
    CASE,
    CONST,
    CONSTRUCTOR,
    CONTINUE,
    DESTRUCTOR,
    DIV,
    DO,
    DOWNTO,
    ELSE,
    END,
    FILE,
    FOR,
    FUNCTION,
    GOTO,
    IF,
    IMPLEMENTATION,
    IN,
    INLINE,
    INTERFACE,
    LABEL,
    MOD,
    NIL,
    OBJECT,
    OF,
    OPERATOR,
    OR,
    PACKED,
    PROCEDURE,
    PROGRAM,
    RECORD,
    REPEAT,
    SET,
    SHL,
    SHR,
    STRING,
    THEN,
    TO,
    TYPE,
    UNIT,
    UNTIL,
    USES,
    VAR,
    WHILE,
    WITH,
    XOR,
    AS,
    CLASS,
    CONSTREF,
    DISPOSE,
    EXCEPT,
    EXIT,
    EXPORTS,
    FINALIZATION,
    FINALLY,
    INHERITED,
    INITIALIZATION,
    IS,
    LIBRARY,
    NEW,
    ON,
    OUT,
    PROPERTY,
    RAISE,
    SELF,
    THREADVAR,
    TRY,

    EOF
}