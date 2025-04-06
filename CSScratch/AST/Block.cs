namespace CSScratch.AST;

public class Block(List<Statement> statements) : Statement
{
    public List<Statement> Statements { get; } = statements;

    public override void Compile(GbWriter writer)
    {
        foreach (var statement in Statements) statement.Compile(writer);
    }
}