namespace CSScratch.AST;

public class Initializer : Statement
{
    public override void Compile(GbWriter writer)
    {
        writer.WriteLine();
        writer.WriteLine("onflag {");
        writer.PushIndent();
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