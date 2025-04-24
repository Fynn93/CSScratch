namespace CSScratch.AST;

public class Include(Name name) : Statement
{
    public Name Name { get; } = name;

    public override void Compile(GbWriter writer)
    {
        var realName = Name.ToString().Split('.').Last().ToLower();
        if (realName == "scratch")
            return;

        writer.WriteLine($"%include lib/{realName}.gs");
    }
}