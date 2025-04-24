using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSScratch.AST;

public class Struct : Statement
{
    public Struct(Name name, List<Statement> statements)
    {
        Name = name;
        Statements = statements;
        AddChild(Name);
        AddChildren(Statements);

        AstUtility.Structs[name.ToString()] = this;
    }

    public Name Name { get; }
    public List<Statement> Statements { get; }

    public override void Compile(GbWriter writer)
    {
        Compile(writer, false);
    }

    public void Compile(GbWriter writer, bool fromCommon)
    {
        if (!fromCommon) return;

        writer.Write("struct ");
        writer.Write(Name.ToString());
        writer.WriteLine(" {");
        writer.PushIndent();
        foreach (var statement in Statements)
        {
            statement.Compile(writer);
        }
        writer.RemoveUpToLastCharacter(',');
        writer.PopIndent();
        writer.WriteLine("}");
        writer.WriteLine(";");
    }

    public static string GetStructInitializer(Struct @struct)
    {
        var returnBody = $"{@struct.Name}{{";
        returnBody = @struct.Statements.Aggregate(returnBody, (current, field) => current + $"{field}: 0, ");
        returnBody = returnBody.TrimEnd(',', ' ');
        returnBody += "}";
        return returnBody;
    }
}