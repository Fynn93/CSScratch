namespace CSScratch.AST;

public class Require(string name, string path) : Statement
{
    public string Name { get; } = name;
    public string Path { get; } = path;

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}