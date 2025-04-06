namespace CSScratch.AST;

public class If : Statement
{
    public If(Expression condition, Statement body, Statement? elseBranch = null)
    {
        Condition = condition;
        Body = body;
        ElseBranch = elseBranch;

        AddChildren([Condition, Body]);
        if (ElseBranch != null) AddChild(ElseBranch);
    }

    public Expression Condition { get; }
    public Statement Body { get; }
    public Statement? ElseBranch { get; }

    public override void Compile(GbWriter writer)
    {
        writer.WriteIf(Condition, Body, ElseBranch);
    }
}