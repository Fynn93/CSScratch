using System.Text.RegularExpressions;
using CSScratch.AST;

namespace CSScratch;

public class GbWriter : BaseWriter
{
    public string Compile(Ast ast)
    {
        ast.Compile(this);
        return ToString();
    }

    public void WriteFunction(string name, string? returnType, List<Parameter> parameters, Block body, bool isPublic = false)
    {
        name = name.Replace(".", "__");
        var funcType = returnType == "void" ? "proc" : "func";
        returnType = returnType switch
        {
            null => "",
            "void" => "",
            "int" => "",
            "string" => "",
            "bool" => "",
            "float" => "",
            "char" => "",
            _ => returnType
        };
        WriteLine($"{funcType} {name}{(funcType == "proc" ? " ": "(")}{string.Join(", ", parameters)}{(funcType == "proc" ? " " : ")")}{returnType}");
        WriteLine("{");
        PushIndent();

        if (parameters.Count > 0)
        {
            // Hack for the parameters
            var newWriter = new GbWriter();
            newWriter.PushIndent();
            body.Compile(newWriter);
            var bodyString = newWriter.ToString();

            var parameterString = string.Join("|", parameters.Select(p => p.Name.Text));
            var regex = new Regex($@"\b(?:{parameterString})\b", RegexOptions.Multiline);
            bodyString = regex.Replace(bodyString, "$$$&");

            WriteLine(bodyString);
        }
        else
        {
            body.Compile(this);
        }

        PopIndent();
        WriteLine("}");

        if (!isPublic) return;
        WriteLine($"on \"{name}\" {{");
        PushIndent();
        foreach (var parameter in parameters)
        {
            WriteLine($"__{name}_p_{parameter.Name.Text}__ = Stack[length Stack];");
            WriteLine("delete Stack[length Stack];");
        }
        Write(name);
        Write(" ");
        foreach (var parameter in parameters)
        {
            Write($"__{name}_p_{parameter.Name.Text}__");
            if (parameter != parameters.Last())
                Write(", ");
        }
        WriteLine(";");
        PopIndent();
        WriteLine("}");
    }

    public void WriteHandler(string eventName, string functionName)
    {
        WriteLine($"on{eventName}");
        WriteLine("{");
        PushIndent();
        WriteLine($"{functionName.Replace(".", "__")};");
        PopIndent();
        WriteLine("}");
    }

    public void WriteIf(Expression condition, Statement body, Statement? elseBranch)
    {
        Write($"if ");
        condition.Compile(this);
        RemoveLastCharacter();
        RemoveLastCharacter();
        RemoveLastCharacter();
        WriteLine(" {");
        PushIndent();
        body.Compile(this);
        PopIndent();
        WriteLine("}");
        if (elseBranch == null) return;
        WriteLine("else {");
        PushIndent();
        elseBranch.Compile(this);
        PopIndent();
        WriteLine("}");
    }

    public void WriteRepeat(Variable? initializer, Expression? condition, Statement? incrementBy, Statement body)
    {
        initializer?.Compile(this);
        Write("repeat ");
        var binaryOperator = (BinaryOperator)condition!;
        WriteLine(binaryOperator.Right.ToString()!);
        WriteLine("{");
        PushIndent();
        body.Compile(this);
        incrementBy?.Compile(this);
        PopIndent();
        WriteLine("}");
    }
}