namespace CSScratch.AST;

public class UnaryOperator : Expression
{
    public UnaryOperator(string @operator, Expression operand)
    {
        Operator = @operator;
        Operand = operand;
        AddChild(Operand);
    }

    public string Operator { get; }
    public Expression Operand { get; }

    public override void Compile(GbWriter writer)
    {
        writer.Write(Operator);
        Operand.Compile(writer);
    }

    public override string ToString()
    {
        return $"{Operator}{Operand}";
    }
}