using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSScratch.AST;

public class Initializer : Statement
{
    public override void Compile(GbWriter writer)
    {
        var fields = Parent!.Descendants.OfType<Assignment>().ToList();
        writer.WriteLine();
        writer.WriteLine("onflag {");
        writer.PushIndent();
        foreach (var field in fields)
        {
            field.Compile(writer, true);
        }
        writer.WriteLine("broadcast_and_wait \"INITIALIZE\";");
        if (AstUtility.EnableTick)
        {
            writer.WriteLine("forever {");
            writer.PushIndent();
            writer.WriteLine("broadcast_and_wait \"TICK\";");
            writer.PopIndent();
            writer.WriteLine("}");
        }
        writer.PopIndent();
        writer.WriteLine("}");
    }
}