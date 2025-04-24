namespace CSScratch.AST;

public class BinaryOperator : Expression
{
    public BinaryOperator(Expression left, string @operator, Expression right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
        AddChildren([Left, Right]);
    }

    public Expression Left { get; set; }
    public string Operator { get; set; }
    public Expression Right { get; set; }

    public override void Compile(GbWriter writer)
    {
        Left.Compile(writer);
        if (writer.GetLastChar() == ';')
            writer.RemoveLastSemicolon();
        writer.Write($" {Operator} ");
        Right.Compile(writer);
        if (writer.GetLastChar() != ';')
            writer.WriteLine(";");
    }

    public override string ToString()
    {
        return $"{Left} {Operator} {Right}";
    }
}