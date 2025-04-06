namespace CSScratch.AST;

public class Initializer : Statement
{
    public override void Compile(GbWriter writer)
    {
        writer.WriteLine("list Stack;");
        writer.WriteLine();
        writer.WriteLine("onflag {");
        writer.PushIndent();
        writer.WriteLine("delete Stack;");
        writer.WriteLine("broadcast_and_wait \"INITIALIZE\";");
        writer.WriteLine("forever {");
        writer.PushIndent();
        writer.WriteLine("broadcast_and_wait \"TICK\";");
        writer.PopIndent();
        writer.WriteLine("}");
        writer.PopIndent();
        writer.WriteLine("}");
    }
}