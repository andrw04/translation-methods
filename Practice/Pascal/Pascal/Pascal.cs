using ConsoleTableExt;

namespace Pascal;

public class Pascal
{
    private static bool _hadError = false;
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            /*            Console.Write("Enter file path: ");
                        var path = Console.ReadLine();*/
            var path = "F:\\BSUIR\\6\\MTran\\Practice\\CodeExamples\\code1.txt";
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

        if (_hadError)
        {
            return;
        }

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
            .WithColumn(new[] { "N", "Token type", "Lexeme", "Line", "Column" })
            .ExportAndWriteLine();
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
}
