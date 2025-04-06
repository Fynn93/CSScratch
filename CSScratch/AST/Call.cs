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
        writer.WriteLine(GetCallAsString(true));
    }

    public override string ToString()
    {
        return GetCallAsString(false);
    }

    private (string,bool) GetCalleeName()
    {
        var callee = Callee.ToString()!.Replace(".", "__");
        callee = callee switch
        {
            "goto_xy" => "goto",
            _ => callee
        };
        var isExternal = callee.Contains(':');
        return (callee.Replace(":", "__"), isExternal);
    }

    private string GetCallAsString(bool fromCompile)
    {
        var returnBody = string.Empty;
        var calleeName = GetCalleeName();
        if (calleeName.Item1.StartsWith('"'))
        {
            // String concatenation
            returnBody = calleeName.Item1.Split("__")[0] + " & " + ArgumentList.Arguments[0];
            return returnBody;
        }
        if (calleeName.Item2)
        {
            returnBody = ArgumentList.Arguments.Aggregate(returnBody, (current, argument) => current + $"add {argument} to Stack;\n");
            returnBody += $"broadcast_and_wait \"{calleeName.Item1}\";\n";
            return returnBody;
        }
        if (fromCompile)
        {
            returnBody += $"{calleeName.Item1} {ArgumentList.ToString().TrimStart('(')[..^1]};";
        }
        else
        {
            returnBody += $"{calleeName.Item1} {ArgumentList}";
        }
        return returnBody;
    }
}