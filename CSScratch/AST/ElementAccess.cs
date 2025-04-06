namespace CSScratch.AST;

public sealed class ElementAccess : Expression
{
    public ElementAccess(Expression expression, Expression index)
    {
        Expression = expression;
        Index = index;
        AddChildren([Expression, Index]);
    }

    public Expression Expression { get; }
    public Expression Index { get; set; }

    public override void Compile(GbWriter writer)
    {
        Expression.Compile(writer);
        writer.WriteLine($"[{Index}]");
    }

    public override string ToString()
    {
        return $"{Expression}[{Index}]";
    }
}