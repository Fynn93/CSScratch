namespace CSScratch.AST;

public sealed class Assignment : Expression
{
    public Assignment(Expression name, Expression value, IdentifierName type = null!)
    {
        Expression = name;
        Value = value;
        Type = type;
        AddChildren([Expression, Value]);
    }

    public Expression Expression { get; }
    public Expression Value;
    public readonly IdentifierName? Type;

    public override void Compile(GbWriter writer)
    {
        CompileAssignment(writer);

        if (Value.ToString()!.Contains('"')) // We have to check for empty string literals
            Value = new Literal($"\"\\\"{Value.ToString()!.Replace("\"", "")}\\\"\""); // And add extra quotes
    }

    public void Compile(GbWriter writer, bool fromInitializer)
    {
        CompileAssignment(writer, fromInitializer);

        if (Value.ToString()!.Contains('"')) // We have to check for empty string literals
            Value = new Literal($"\"\\\"{Value.ToString()!.Replace("\"", "")}\\\"\""); // And add extra quotes
    }

    private void CompileAssignment(GbWriter writer, bool fromInitializer = false)
    {
        switch (Expression)
        {
            case Name name:
                writer.WriteLine($"{name} = {Value};");
                return;
            case MemberAccess memberAccess:
                if (!fromInitializer)
                    return;
                var type = "";
                if (Type is not null)
                {
                    type = AstUtility.IsBaseType(Type!.Text) ? "" : Type.Text;
                }
                writer.Write($"{type} {memberAccess.Name}");
                break;
            default:
                Expression.Compile(writer);
                break;
        }
        writer.Write(" = ");
        Value.Compile(writer);
        writer.WriteLine(";");
    }
}