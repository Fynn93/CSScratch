namespace CSScratch.AST;

/// <summary>Simply renders nothing.</summary>
public sealed class NoOp : Statement
{
    public override void Compile(GbWriter writer)
    {
    }
}