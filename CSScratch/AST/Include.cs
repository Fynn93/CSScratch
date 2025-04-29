namespace CSScratch.AST;

public class Include(Name name) : Statement
{
    public Name Name { get; } = name;

    public override void Compile(GbWriter writer)
    {
        var realName = Name.ToString().Split('.').Last().ToLower();
        if (realName is "scratch" or "stage")
            return;

        var library = Name.ToString().Contains("Library");
        writer.WriteLine($"%include {(library ? "Library/" : "")}{realName}.gs");
    }
}