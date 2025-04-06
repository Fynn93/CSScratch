namespace CSScratch.AST;

public class Repeat : Statement
{
    public Repeat(Expression untilCondition, Statement body)
    {
        UntilCondition = untilCondition;
        Body = body;
        AddChild(untilCondition);
        AddChild(body);
    }

    public Expression UntilCondition { get; }
    public Statement Body { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}