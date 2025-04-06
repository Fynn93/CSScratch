namespace CSScratch.AST;

public class Parenthesized(Expression expression) : Expression
{
    public Expression Expression { get; } = expression;

    public override void Compile(GbWriter writer)
    {
        Expression.Compile(writer);
    }
}