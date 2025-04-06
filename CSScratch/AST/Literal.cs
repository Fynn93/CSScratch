namespace CSScratch.AST;

public class Literal(string valueText) : Expression
{
    public string ValueText { get; } = valueText;

    public override void Compile(GbWriter writer)
    {
        writer.Write(ValueText);
    }

    public override string ToString()
    {
        return ValueText;
    }
}