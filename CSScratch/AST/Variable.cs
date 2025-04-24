namespace CSScratch.AST;

public sealed class Variable : Statement
{
    public Expression? Initializer;

    public Variable(IdentifierName name, bool isLocal, Expression? initializer = null, TypeRef? type = null, bool structEntry = false)
    {
        Name = name;
        IsLocal = isLocal;
        Initializer = initializer;
        Type = type;
        StructEntry = structEntry;

        AddChild(Name);
        if (Initializer != null) AddChild(Initializer);
        if (Type != null) AddChild(Type);
    }

    public IdentifierName Name { get; }
    public bool IsLocal { get; }
    public bool StructEntry { get; }
    public TypeRef? Type { get; }

    public override void Compile(GbWriter writer)
    {
        if (IsLocal)
        {
            writer.Write("local ");
        }
        Type?.Compile(writer);
        if (Initializer is Call call2)
        {
            var splitted = call2.Callee.ToString()!.Split('.');
            if (splitted is [_, _, "new", ..])
            {
                var typeName = splitted[1];
                writer.Write(typeName);
                writer.Write(" ");
            }
        }
        Name.Compile(writer);
        if (StructEntry)
            writer.WriteLine(",");
        if (Initializer == null) return;

        writer.Write(" = ");
        if (Initializer is Call call)
        {
            var hasReturn = false;
            var returnType = "";
            if (AstUtility.Functions.TryGetValue(call.Callee.ToString()!.Replace(':', '.'), out var function))
            {
                if (function.ReturnType != null && function.ReturnType.Path != "void")
                {
                    hasReturn = true;
                    returnType = function.ReturnType.ToString().Trim();
                }
            }

            if (hasReturn)
            {
                call.Compile(writer);
                if (call.IsExternal(call.Callee.ToString()!))
                {
                    writer.WriteLine($"{Name} = __{returnType}_ret__;");
                }
                return;
            }
        }

        Initializer.Compile(writer);
        writer.WriteLine(";");
    }

    public override string ToString()
    {
        return Name.Text;
    }
}
