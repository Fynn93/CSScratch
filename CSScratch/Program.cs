#region

using System.Diagnostics;
using CSScratch;
using CSScratch.AST;
using Microsoft.CodeAnalysis.CSharp;

#endregion

var sourceDir = args.Length > 0 ? args[0] : "source";
var outputDir = args.Length > 1 ? args[1] : "output";

Console.ForegroundColor = ConsoleColor.Blue;

var started = DateTime.Now;
Console.WriteLine("Compiling project...");
Console.WriteLine($"Source directory: {sourceDir}");
Console.WriteLine($"Output directory: {outputDir}");

if (!Directory.Exists(sourceDir))
    Directory.CreateDirectory(sourceDir);

if (!Directory.Exists(outputDir))
    Directory.CreateDirectory(outputDir);

foreach (var file in Directory.EnumerateFiles(sourceDir, "*.cs", SearchOption.AllDirectories))
{
    if (file.Contains("bin") || file.Contains("obj"))
        continue;

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"Compiling {Path.GetFileNameWithoutExtension(file)}...");
    var source = File.ReadAllText(file);
    var references = FileUtility.GetCompilationReferences();
    var sourceAst = CSharpSyntaxTree.ParseText(source);
    var compiler = CSharpCompilation.Create(
        "Compiled",
        [sourceAst],
        references
    );

    foreach (var diagnostic in compiler.GetDiagnostics().Where(diagnostic => diagnostic.Id != "CS5001"))
        Logger.HandleDiagnostic(diagnostic, Path.GetFileNameWithoutExtension(file));

    var generator = new AstGenerator(sourceAst, compiler);
    Ast ast;
    try
    {
        ast = generator.GetAst();
    }
    catch (Exception e)
    {
        Logger.Error($"Failed to generate AST for {file}: {e}");
        continue;
    }

    var writer = new GbWriter();
    var compiled = writer.Compile(ast);
    var outputFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(file).ToLower() + ".gs");
    File.WriteAllText(outputFile, compiled);
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Compilation completed.");
Console.WriteLine($@"Time elapsed: {DateTime.Now - started:s\.FFFFFFF}s");

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Running goboscript build...");
var startInfo = new ProcessStartInfo
{
    FileName = "goboscript",
    Arguments = $"build",
    WorkingDirectory = outputDir,
    RedirectStandardError = true,
    UseShellExecute = false,
    CreateNoWindow = true
};
var process = new Process
{
    StartInfo = startInfo,
};
process.Start();
// Error has the output
var error = process.StandardError.ReadToEnd();
process.WaitForExit();
if (process.ExitCode != 0)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Goboscript build failed: {error}");
}
else
{
    Console.Write(error);
}
Console.ResetColor();