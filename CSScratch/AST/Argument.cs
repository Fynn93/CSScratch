namespace CSScratch.AST;

public class Argument : Expression
{
    public Argument(Expression expression)
    {
        Expression = expression;
        AddChild(Expression);
    }

    public Expression Expression { get; set; }

    public override void Compile(GbWriter writer)
    {
        Expression.Compile(writer);
    }

    public override string ToString()
    {
        return Expression.ToString()!;
    }
}