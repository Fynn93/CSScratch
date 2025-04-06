namespace CSScratch.AST;

public class MemberAccess : Expression
{
    public MemberAccess(Expression expression, IdentifierName name, char @operator = '.')
    {
        Expression = expression;
        Operator = @operator;
        Name = name;
        AddChildren([Expression, Name]);
    }

    public Expression Expression { get; }
    public char Operator { get; set; }
    public IdentifierName Name { get; }

    public override void Compile(GbWriter writer)
    {
        if (Expression is Name name)
        {
            writer.WriteLine($"{name}{Operator}{Name};");
        }
        else
        {
            Expression.Compile(writer);
            writer.Write($" {Operator} ");
            Name.Compile(writer);
        }
    }

    public override string ToString()
    {
        return Name.Text == "Length" ? $"length {Expression}" : $"{Expression}{Operator}{Name}";
    }
}