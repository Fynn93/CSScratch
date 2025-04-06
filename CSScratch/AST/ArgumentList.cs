namespace CSScratch.AST;

public class ArgumentList : Expression
{
    public ArgumentList(List<Argument> arguments)
    {
        Arguments = arguments;
        AddChildren(Arguments.OfType<Node>().ToList());
    }

    public List<Argument> Arguments { get; set; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"({string.Join(", ", Arguments)})";
    }
}