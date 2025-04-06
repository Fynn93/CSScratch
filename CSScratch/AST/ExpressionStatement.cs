namespace CSScratch.AST;

public class ExpressionStatement : Statement
{
    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
        AddChild(Expression);
    }

    public Expression Expression { get; }

    public override void Compile(GbWriter writer)
    {
        Expression.Compile(writer);
    }
}