#region

using Microsoft.CodeAnalysis;

#endregion

namespace CSScratch;

public static class Logger
{
    public static void Ok(string message)
    {
        Log(message, ConsoleColor.Green, "OK");
    }

    public static void Info(string message)
    {
        Log(message, ConsoleColor.Blue, "INFO");
    }

    public static Exception Error(string message)
    {
        Log(message, ConsoleColor.Red, "ERROR");
        return new Exception();
    }

    public static Exception CompilerError(string message)
    {
        return Error($"{message} (csscratch compiler error)");
    }

    public static Exception CodegenError(SyntaxToken token, string message)
    {
        var lineSpan = token.GetLocation().GetLineSpan();
        return Error($"{message}\n\t- {FormatLocation(lineSpan)}");
    }

    public static void CodegenWarning(SyntaxToken token, string message)
    {
        var lineSpan = token.GetLocation().GetLineSpan();
        Warn($"{message}\n\t- {FormatLocation(lineSpan)}");
    }

    public static Exception UnsupportedError(SyntaxNode node, string subject, bool useIs = false, bool useYet = true)
    {
        return CodegenError(node, $"{subject} {(useIs ? "is" : "are")} not {(useYet ? "yet" : "")} supported, sorry!");
    }

    public static Exception CodegenError(SyntaxNode node, string message)
    {
        return CodegenError(node.GetFirstToken(), message);
    }

    public static void CodegenWarning(SyntaxNode node, string message)
    {
        CodegenWarning(node.GetFirstToken(), message);
    }

    public static void HandleDiagnostic(Diagnostic diagnostic, string file)
    {
        HashSet<string> ignoredCodes = ["CS7022", "CS0017", "CS0246", "CS1980", "CS0176", "CS0103"];
        if (ignoredCodes.Contains(diagnostic.Id)) return;

        var lineSpan = diagnostic.Location.GetLineSpan();
        var diagnosticMessage = $"{diagnostic.Id}: {diagnostic.GetMessage()}";
        var location = $"\n\t- {FormatLocation(lineSpan, file)}";
        switch (diagnostic.Severity)
        {
            case DiagnosticSeverity.Error:
            {
                Error(diagnosticMessage + location);
                break;
            }
            case DiagnosticSeverity.Warning:
            {
                if (diagnostic.IsWarningAsError)
                    Error(diagnosticMessage + location);
                else
                    Warn(diagnosticMessage + location);
                break;
            }
            case DiagnosticSeverity.Info:
            {
                Info(diagnosticMessage);
                break;
            }
        }
    }

    public static void Warn(string message)
    {
        Log(message, ConsoleColor.Yellow, "WARN");
    }

    public static void Debug(string message)
    {
        Log(message, ConsoleColor.Magenta, "DEBUG");
    }

    private static void Log(string message, ConsoleColor color, string level)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"[{level}] {message}");
        Console.ForegroundColor = originalColor;
    }

    private static string FormatLocation(FileLinePositionSpan lineSpan, string? filename = null)
    {
        filename ??= lineSpan.Path == "" ? "<anonymous>" : lineSpan.Path;

        return
            $"{filename}:{lineSpan.StartLinePosition.Line + 1}:{lineSpan.StartLinePosition.Character + 1}";
    }
}