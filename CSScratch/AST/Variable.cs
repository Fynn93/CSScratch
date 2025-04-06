namespace CSScratch.AST;

public sealed class Variable : Statement
{
    public Expression? Initializer;

    public Variable(IdentifierName name, bool isLocal, Expression? initializer = null, TypeRef? type = null)
    {
        Name = name;
        IsLocal = isLocal;
        Initializer = initializer;
        Type = type;

        AddChild(Name);
        if (Initializer != null) AddChild(Initializer);
        if (Type != null) AddChild(Type);
    }

    public IdentifierName Name { get; }
    public bool IsLocal { get; }
    public TypeRef? Type { get; }

    public override void Compile(GbWriter writer)
    {
        if (IsLocal)
        {
            writer.Write("local ");
        }
        Name.Compile(writer);
        if (Initializer == null) return;

        writer.Write(" = ");
        Initializer.Compile(writer);
        writer.WriteLine(";");
    }
}