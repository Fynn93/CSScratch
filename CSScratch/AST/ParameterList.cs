namespace CSScratch.AST;

public class ParameterList : Statement
{
    public ParameterList(List<Parameter> parameters)
    {
        Parameters = parameters;
        AddChildren(Parameters);
    }

    public List<Parameter> Parameters { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}