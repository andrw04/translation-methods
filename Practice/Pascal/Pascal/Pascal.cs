using Pascal.LexicalAnalysis;
using ConsoleTableExt;
using Pascal.SyntacticAnalysis;
using Pascal.SyntacticAnalysis.Expressions;

namespace Pascal;

public class Pascal
{
    private static bool _hadError = false;
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            var path = "F:\\BSUIR\\6\\MTran\\Practice\\CodeExamples\\code2.txt";
            RunFile(path);

        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        } 
        else
        {
            Console.WriteLine("Usage: csharppascal [script]");
            return;
        }
    }
    
    private static void RunFile(string path)
    {
        // читаем файл и запсукаем Run
        if (File.Exists(path))
        {
            string source = string.Join('\n', File.ReadAllLines(path));
            Run(source);
        }
        else
        {
            Console.WriteLine("File doesn't exist.");
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new Parser(tokens);
        var statements = parser.Parse();

        if (_hadError)
        {
            return;
        }

        var printer = new Printer();
        Console.WriteLine(printer.Print(statements));

        //PrintTokens(tokens);
    }

    public static void Error(int line, int column, String message)
    {
        Report(line, column, "", message);
    }

    private static void Report(int line, int column, String where, String message)
    {
        Console.WriteLine($"[line {line}][Column {column}] Error {where}: {message}");
        _hadError = true;
    }

    public static void Error(Token token, String message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, token.Column, " at end", message);
        }
        else
        {
            Report(token.Line, token.Column, " at '" + token.Lexeme + "'", message);
        }
    }

    public static void PrintTokens(List<Token> tokens)
    {
        var tableData = new List<List<object>>();

        for (int i = 0; i < tokens.Count; i++)
        {
            tableData.Add(new List<object>()
            {
                i,
                tokens[i].Type,
                tokens[i].Lexeme,
                tokens[i].Line,
                tokens[i].Column
            });
        }

        ConsoleTableBuilder
            .From(tableData)
            .WithTitle("Token table")
            .WithColumn(new[] { "N", "Token type", "Lexeme", "Line", "Column" })
            .ExportAndWriteLine();
    }
}
