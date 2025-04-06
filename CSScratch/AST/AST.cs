namespace CSScratch.AST;

public class Ast : Node
{
    public Ast(List<Statement> statements)
    {
        Statements = statements;
        AddChildren(Statements);
    }

    public List<Statement> Statements { get; }

    public override void Compile(GbWriter writer)
    {
        // TODO: Implement this
        writer.WriteLine("costumes \"blank.svg\";\n");
        writer.WriteLine("# Compiled with CSScratch v" + GetType().Assembly.GetName().Version);
        foreach (var statement in Statements) statement.Compile(writer);
    }
}