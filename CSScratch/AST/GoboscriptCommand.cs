namespace CSScratch.AST;

public class GoboscriptCommand(string command) : Statement
{
    public string Command { get; set; } = command;

    public override void Compile(GbWriter writer)
    {
        writer.WriteLine(Command);
    }
}