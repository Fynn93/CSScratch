namespace CSScratch.AST;

public class VariableList : Statement
{
    public VariableList(List<Variable> variables)
    {
        Variables = variables;
        AddChildren(Variables);
    }

    public List<Variable> Variables { get; }

    public override void Compile(GbWriter writer)
    {
        foreach (var variable in Variables) variable.Compile(writer);
    }
}