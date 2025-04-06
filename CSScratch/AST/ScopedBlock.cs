namespace CSScratch.AST;

public class ScopedBlock(List<Statement> statements) : Block(statements)
{
    public override void Compile(GbWriter writer)
    {
        base.Compile(writer);
    }
}