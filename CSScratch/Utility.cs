#region

using System.Reflection;
using System.Text.RegularExpressions;
using CSScratch.AST;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CSScratch.Constants;

#endregion

namespace CSScratch;

public static partial class Utility
{
    public const string RuntimeAssemblyName = "CSScratch.Library";

    public static string GetDefaultValueForType(string typeName)
    {
        if (IntegerTypes.Contains(typeName) || DecimalTypes.Contains(typeName)) return "0";

        return typeName switch
        {
            "char" or "Char" or "string" or "String" => "\"\"",
            "bool" or "Boolean" => "false",
            _ => "void"
        };
    }

    public static ISymbol? FindMember(INamespaceSymbol namespaceSymbol, string memberName)
    {
        var member = namespaceSymbol.GetMembers().FirstOrDefault<ISymbol?>(member => member?.Name == memberName, null);
        if (member == null && namespaceSymbol.ContainingNamespace != null)
            member = FindMember(namespaceSymbol.ContainingNamespace, memberName);
        return member;
    }

    public static ISymbol? FindMemberDeep(INamedTypeSymbol namedTypeSymbol, string memberName)
    {
        var member = namedTypeSymbol.GetMembers().FirstOrDefault(member => member.Name == memberName);
        if (namedTypeSymbol.BaseType != null && member == null)
            return FindMemberDeep(namedTypeSymbol.BaseType, memberName);
        return member;
    }

    public static void PrettyPrint(object? obj)
    {
        if (obj == null)
        {
            Console.WriteLine("null");
            return;
        }

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var result = $"{type.Name}:\n";

        foreach (var property in properties)
        {
            var value = property.GetValue(obj, null);
            result += $"  {property.Name}: {value}\n";
        }

        Console.WriteLine(result);
    }

    public static List<T> FilterDuplicates<T>(IEnumerable<T> items, IEqualityComparer<T> comparer) where T : notnull
    {
        var seen = new Dictionary<T, bool>(comparer);

        return items.Where(item => seen.TryAdd(item, true)).ToList();
    }

    public static string GetMappedType(string csharpType)
    {
        if (csharpType.EndsWith("[]"))
        {
            var arrayType = csharpType[..^2];
            return $"{{ {GetMappedType(arrayType)} }}";
        }

        if (csharpType.EndsWith('?'))
        {
            var nonNullableType = csharpType[..^1];
            return $"{GetMappedType(nonNullableType)}?";
        }

        if (csharpType.StartsWith("Action<") || csharpType == "Action")
        {
            var typeArgs = ExtractTypeArguments(csharpType).Select(GetMappedType);
            return $"({string.Join(", ", typeArgs)}) -> void";
        }

        if (csharpType.StartsWith("Func<"))
        {
            var typeArgs = ExtractTypeArguments(csharpType).Select(GetMappedType);
            var enumerable = typeArgs.ToList();
            var returnType = enumerable.Last();
            typeArgs = enumerable.SkipLast(1).ToList();
            return $"({string.Join(", ", typeArgs)}) -> {returnType}";
        }

        return csharpType switch
        {
            "object" => "",
            "void" or "null" => "void",
            "char" or "Char" or "String" => "",
            "bool" => "",
            _ => IntegerTypes.Contains(csharpType) ? "" : csharpType
        };
    }

    public static string GetMappedOperator(string op)
    {
        return op switch
        {
            "++" => "+=",
            "--" => "-=",
            "!" => "not ",
            "!=" => "~=",
            "&&" => "and",
            "||" => "or",
            _ => op
        };
    }

    public static List<string> ExtractTypeArguments(string input)
    {
        var typeArguments = new List<string>();
        var regex = ArgRegex();
        var match = regex.Match(input);
        if (!match.Success) return typeArguments;
        var args = match.Groups["args"].Value;
        var argsArray = args.Split([','], StringSplitOptions.RemoveEmptyEntries);
        typeArguments.AddRange(argsArray.Select(arg => arg.Trim()));

        return typeArguments;
    }

    public static List<string> GetNamesFromNode(SyntaxNode? node)
    {
        if (node is BaseExpressionSyntax baseExpression) return [""];

        var names = new List<string>();
        if (node == null) return names;

        var identifierProperty = node.GetType().GetProperty("Identifier");
        var identifierValue = identifierProperty?.GetValue(node);
        if (identifierProperty != null && identifierValue is SyntaxToken token)
        {
            names.Add(token.ValueText.Trim());
            return names;
        }

        var childNodes = node.ChildNodes();
        var syntaxNodes = childNodes.ToList();
        var qualifiedNameNodes = node.IsKind(SyntaxKind.QualifiedName)
            ? [(QualifiedNameSyntax)node]
            : syntaxNodes.OfType<QualifiedNameSyntax>();
        var identifierNameNodes = node.IsKind(SyntaxKind.IdentifierName)
            ? [(IdentifierNameSyntax)node]
            : syntaxNodes.OfType<IdentifierNameSyntax>();
        foreach (var qualifiedNameNode in qualifiedNameNodes)
        {
            names.AddRange(GetNamesFromNode(qualifiedNameNode.Left).Select(name => name.Trim()));
            names.AddRange(GetNamesFromNode(qualifiedNameNode.Right).Select(name => name.Trim()));
        }

        names.AddRange(identifierNameNodes.Select(identifierNameNode =>
            identifierNameNode.Identifier.ValueText.Trim()));

        return names;
    }

    public static BinaryOperator FixArgumentListConcatenationOperators(BinaryOperator binaryOperator)
    {
        if (binaryOperator.Operator != "+") return binaryOperator;
        //binaryOperator.Operator = "..";

        if (binaryOperator.Left is BinaryOperator leftBinaryOperatorLeft)
            FixArgumentListConcatenationOperators(leftBinaryOperatorLeft);

        if (binaryOperator.Right is BinaryOperator leftBinaryOperatorRight)
            FixArgumentListConcatenationOperators(leftBinaryOperatorRight);

        return binaryOperator;
    }

    [GeneratedRegex(@"<(?<args>[^<>]+)>")]
    private static partial Regex ArgRegex();
}