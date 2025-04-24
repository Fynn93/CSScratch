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

    public void WriteFunction(string name, string? returnType, List<Parameter> parameters, Block body, bool isPublic = false, bool noWarp = false)
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
        if (returnType.Contains('.'))
            returnType = returnType.Split('.').Last();
        Write($"{(funcType == "proc" && noWarp ? "nowarp ": "")}");
        WriteLine($"{funcType} {name}{(funcType == "proc" ? " ": "(")}{string.Join(", ", parameters)}{(funcType == "proc" ? " " : ")")} {returnType} {{");
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
        //TODO
        return;
        WriteLine($"on \"{name}\" {{");
        PushIndent();
        var iterator = 0;
        foreach (var parameter in parameters)
        {
            var type = AstUtility.IsBaseType(parameter.Type!) ? "" : parameter.Type!.ToString().Trim();
            WriteLine($"{type} __{name}_p_{parameter.Name.Text}__ = __{type}_arg_{iterator}__;");
            iterator++;
        }
        var customReturnType = AstUtility.IsBaseType(returnType) ? "" : returnType;
        if (funcType != "proc")
            Write($"__{customReturnType}_ret__ = ");
        Write(name);
        Write((funcType == "proc" ? " ": "("));
        foreach (var parameter in parameters)
        {
            Write($"__{name}_p_{parameter.Name.Text}__");
            if (parameter != parameters.Last())
                Write(", ");
        }
        Write((funcType == "proc" ? " " : ")"));
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
        WriteLine(";");
        PopIndent();
        WriteLine("}");
    }

    /// Gets the last character of the output string and skips new lines and carriage returns
    public char GetLastChar()
    {
        if (_output.Length == 0) return ' ';
        for (var i = _output.Length - 1; i >= 0; i--)
        {
            if (_output[i] != '\n' && _output[i] != '\r' && _output[i] != ' ')
                return _output[i];
        }
        return ' ';
    }

    public void RemoveLastSemicolon() => RemoveUpToLastCharacter(';');

    public void RemoveUpToLastCharacter(char character)
    {
        // Removes the last semicolon from the output string
        if (_output.Length == 0) return;
        for (var i = _output.Length - 1; i >= 0; i--)
        {
            if (_output[i] != character)
            {
                RemoveLastCharacter();
                continue;
            }
            RemoveLastCharacter();
            break;
        }
    }
}