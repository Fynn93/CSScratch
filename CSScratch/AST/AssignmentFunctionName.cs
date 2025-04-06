namespace CSScratch.AST;

public class AssignmentFunctionName : Name
{
    public AssignmentFunctionName(Name left, IdentifierName right, char @operator = '.')
    {
        Left = left;
        Right = right;
        Operator = @operator;
        AddChildren([Left, Right]);
    }

    public Name Left { get; }
    public char Operator { get; set; }
    public IdentifierName Right { get; }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return Left.ToString() + Operator + Right;
    }
}