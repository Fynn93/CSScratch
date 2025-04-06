namespace CSScratch.AST;

public class Return : Statement
{
    public Return(Expression? expression = null)
    {
        Expression = expression;
        if (Expression != null) AddChild(Expression);
    }

    public Expression? Expression { get; }

    public override void Compile(GbWriter writer)
    {
        writer.Write("return");
        if (Expression != null)
        {
            writer.Write(" ");
            Expression.Compile(writer);
        }
        writer.WriteLine(";");
    }
}