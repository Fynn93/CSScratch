namespace CSScratch.AST;

public class TypeRef(string path, bool rawPath = false) : Expression
{
    public string Path { get; protected init; } =
        rawPath ? path.Trim(';') : AstUtility.CreateTypeRef(path)!.Path.Trim(';');

    public bool IsNullable { get; protected set; }

    public override void Compile(GbWriter writer)
    {
        writer.Write(path);
        writer.Write(" ");
    }

    public override string ToString()
    {
        return path + " ";
    }
}