namespace CSScratch.AST;

public class Initializer : Statement
{
    public override void Compile(GbWriter writer)
    {
        writer.WriteLine("list Stack;");
        writer.WriteLine($"var ___ret__ = 0;");
        for (var i = 0; i < 9; i++)
            writer.WriteLine($"var ___arg_{i}__ = 0;");

        foreach (var @struct in AstUtility.Structs)
        {
            @struct.Value.Compile(writer);
        }

        writer.WriteLine();
        writer.WriteLine("onflag {");
        writer.PushIndent();
        writer.WriteLine("delete Stack;");
        foreach (var @struct in AstUtility.Structs)
        {
            writer.WriteLine($"{@struct.Key} __{@struct.Key}_ret__ = {Struct.GetStructInitializer(@struct.Value)};");
            for (var i = 0; i < 9; i++)
                writer.WriteLine($"{@struct.Key} __{@struct.Key}_arg_{i}__ = {Struct.GetStructInitializer(@struct.Value)};");
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