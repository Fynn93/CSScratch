namespace CSScratch.AST;

public class Parameter : Statement
{
    public Parameter(IdentifierName name, bool isVararg = false, Expression? initializer = null, TypeRef? type = null)
    {
        Name = name;
        Initializer = initializer;
        Type = type;
        IsVararg = isVararg;

        AddChild(Name);
        if (Initializer != null) AddChild(Initializer);

        if (Type == null) return;
        Type = FixType(Type);
        AddChild(Type);
    }

    public IdentifierName Name { get; }
    public Expression? Initializer { get; }
    public TypeRef? Type { get; }
    public bool IsVararg { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }

    private TypeRef FixType(TypeRef type)
    {
        while (true)
        {
            if (type is ArrayType arrayType && IsVararg)
            {
                type = arrayType.ElementType;
                continue;
            }

            var isOptional = type is OptionalType;
            if (Initializer != null || isOptional) return isOptional ? type : new OptionalType(type);

            return type;
        }
    }

    public override string ToString()
    {
        var typeStr = Type?.ToString().Trim();
        typeStr = typeStr switch
        {
            null => "",
            "void" => "",
            "int" => "",
            "string" => "",
            "bool" => "",
            "float" => "",
            "char" => "",
            "dynamic" => "",
            _ => typeStr
        };
        return (typeStr + " " + Name.Text).Trim();
    }
}