namespace CSScratch.AST;

public class While : Statement
{
    public While(Expression condition, Statement body)
    {
        Condition = condition;
        Body = body;
        AddChildren([Condition, Body]);
    }

    public Expression Condition { get; }
    public Statement Body { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}