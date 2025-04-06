namespace CSScratch.AST;

public class ExpressionalIf : Expression
{
    public ExpressionalIf(Expression condition, Expression body, Expression? elseBranch = null)
    {
        Condition = condition;
        Body = body;
        ElseBranch = elseBranch;

        AddChild(Condition);
        AddChild(Body);
        if (ElseBranch != null) AddChild(ElseBranch);
    }

    public Expression Condition { get; }
    public Expression Body { get; }
    public Expression? ElseBranch { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}