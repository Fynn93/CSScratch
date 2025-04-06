namespace CSScratch.AST;

public class NumericalFor : Statement
{
    public NumericalFor(Variable? initializer, Expression? incrementBy, Expression? condition, Statement body)
    {
        Initializer = initializer;
        Condition = condition ?? new Literal("true");
        IncrementBy = incrementBy != null ? new ExpressionStatement(incrementBy) : null;
        Body = body;

        AddChildren([Condition, Body]);
        if (Initializer != null) AddChild(Initializer);
        if (IncrementBy != null) AddChild(IncrementBy);
    }

    public Variable? Initializer { get; }
    public Expression Condition { get; }
    public Statement? IncrementBy { get; }
    public Statement Body { get; }

    public override void Compile(GbWriter writer)
    {
        writer.WriteRepeat(Initializer, Condition, IncrementBy, Body);
    }
}