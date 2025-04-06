namespace CSScratch.AST;

public class IdentifierName(string text) : Name
{
    public string Text { get; } = text;

    public override void Compile(GbWriter writer)
    {
        writer.Write(Text);
    }

    public override string ToString()
    {
        return Text;
    }
}