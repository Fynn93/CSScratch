namespace CSScratch.AST;

public class AnonymousFunction : Expression
{
    public AnonymousFunction(ParameterList parameterList, Block? body = null)
    {
        ParameterList = parameterList;
        Body = body;
        AddChild(ParameterList);
        if (Body != null) AddChild(Body);
    }

    public ParameterList ParameterList { get; }
    public Block? Body { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}