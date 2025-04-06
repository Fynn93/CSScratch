namespace CSScratch.AST;

public sealed class TypeAlias : Statement
{
    public TypeAlias(IdentifierName name, TypeRef type, bool export = false)
    {
        Name = name;
        Type = type;
        Export = export;

        AddChildren([Name, Type]);
    }

    public IdentifierName Name { get; }
    public TypeRef Type { get; }
    public bool Export { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}