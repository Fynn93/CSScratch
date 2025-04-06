namespace CSScratch.AST;

public class For : Statement
{
    public For(List<IdentifierName> initializers, Expression iterator, Statement body)
    {
        Names = initializers;
        Iterator = iterator;
        Body = body;
        AddChildren(Names);
        AddChild(Iterator);
        AddChild(Body);
    }

    public List<IdentifierName> Names { get; }
    public Expression Iterator { get; }
    public Statement Body { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}