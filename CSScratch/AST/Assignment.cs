namespace CSScratch.AST;

public sealed class Assignment : Expression
{
    public Expression Value;

    public Assignment(Expression name, Expression value)
    {
        Expression = name;
        Value = value;
        AddChildren([Expression, Value]);
    }

    public Expression Expression { get; }

    public override void Compile(GbWriter writer)
    {
        switch (Expression)
        {
            case Name name:
                writer.WriteLine($"{name} = {Value};");
                return;
            case MemberAccess memberAccess:
                writer.Write($"var {memberAccess.Name}");
                break;
            default:
                Expression.Compile(writer);
                break;
        }

        writer.Write(" = ");
        if (Value.ToString()!.Contains('"')) // We have to check for empty string literals
            Value = new Literal($"\"\\\"{Value.ToString()!.Replace("\"", "")}\\\"\"");   // And add extra quotes
        Value.Compile(writer);
        writer.WriteLine(";");
    }
}