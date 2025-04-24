namespace CSScratch.AST;

public sealed class Call : Expression
{
    public Call(Expression callee, ArgumentList argumentList)
    {
        Callee = callee;
        ArgumentList = argumentList;
        AddChildren([Callee, ArgumentList]);
    }

    public Expression Callee { get; }
    public ArgumentList ArgumentList { get; }

    public override void Compile(GbWriter writer)
    {
        writer.WriteLine(GetCallAsString(true, writer));
    }

    public override string ToString()
    {
        return GetCallAsString(false);
    }

    public bool IsExternal(string callee)
    {
        return callee.Contains(':');
    }

    private (string,bool) GetCalleeName()
    {
        var callee = Callee.ToString()!.Replace(".", "__");
        callee = callee switch
        {
            "goto_xy" => "goto",
            _ => callee
        };
        var isExternal = IsExternal(callee);
        return (callee.Replace(":", "__"), isExternal);
    }

    private string GetCallAsString(bool fromCompile, GbWriter writer = null!)
    {
        var returnBody = string.Empty;
        var calleeName = GetCalleeName();
        if (calleeName.Item1.EndsWith("__new"))
        {
            // Constructor
            var type = calleeName.Item1.Split("__")[1];
            var @struct = AstUtility.Structs[type];
            returnBody = Struct.GetStructInitializer(@struct);
            return returnBody;
        }
        if (calleeName.Item1.StartsWith('"'))
        {
            // String concatenation
            returnBody = calleeName.Item1.Split("__")[0] + " & " + ArgumentList.Arguments[0];
            return returnBody;
        }

        var hasReturn = false;
        if (AstUtility.Functions.TryGetValue(Callee.ToString()!.Replace(':', '.'), out var function))
        {
            if (function.ReturnType != null && function.ReturnType.Path != "void")
            {
                hasReturn = true;
            }
        }
        if (fromCompile)
        {
            returnBody += $"{calleeName.Item1}{(hasReturn ? "(" : " ")}{ArgumentList.ToString().TrimStart('(')[..^1]}{(hasReturn ? ")" : " ")};";
        }
        else
        {
            returnBody += $"{calleeName.Item1}{(hasReturn ? "(" : " ")}{ArgumentList}{(hasReturn ? ")" : " ")}";
        }
        return returnBody;
    }
}