namespace CSScratch.AST;

public class Block : Statement
{
    public Block(List<Statement> statements)
    {
        Statements = statements;
        AddChildren(Statements);
    }

    public List<Statement> Statements { get; }

    public override void Compile(GbWriter writer)
    {
        foreach (var statement in Statements) statement.Compile(writer);
    }

    public override string ToString()
    {
        return Statements.Aggregate(string.Empty, (current, statement) => current + statement);
    }
}