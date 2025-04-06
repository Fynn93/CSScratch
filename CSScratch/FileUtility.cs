#region

using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

#endregion

namespace CSScratch;

public static partial class FileUtility
{
    public static string FixPathSep(string path)
    {
        path = Path.TrimEndingDirectorySeparator(path);
        return PathSepRegex().Replace(path.Replace(@"\\", "/").Replace('\\', '/').Replace("//", "/"), "");
    }

    public static string GetAssemblyDirectory()
    {
        var location = FixPathSep(Assembly.GetExecutingAssembly().Location);
        var directoryName = Path.GetDirectoryName(location)!;
        return FixPathSep(directoryName);
    }

    public static List<PortableExecutableReference> GetCompilationReferences()
    {
        var runtimeLibAssemblyPath = string.Join('/', GetAssemblyDirectory(), Utility.RuntimeAssemblyName + ".dll");
        if (!File.Exists(runtimeLibAssemblyPath))
        {
            var directoryName = Path.GetDirectoryName(runtimeLibAssemblyPath);
            Logger.Error(
                $"Failed to find {Utility.RuntimeAssemblyName}.dll in {(directoryName == null ? "(could not find assembly directory)" : FixPathSep(directoryName))}");
        }

        var references = new List<PortableExecutableReference>
        {
            MetadataReference.CreateFromFile(runtimeLibAssemblyPath)
        };
        references.AddRange(GetCoreLibReferences());

        return references;
    }

    public static HashSet<PortableExecutableReference> GetCoreLibReferences()
    {
        var coreLib = typeof(object).GetTypeInfo().Assembly.Location;
        HashSet<string> coreDlls = ["System.Runtime.dll", "System.Core.dll", "System.Collections.dll"];
        HashSet<PortableExecutableReference> references = [MetadataReference.CreateFromFile(coreLib)];

        foreach (var dllPath in coreDlls.Select(coreDll => Path.Combine(Path.GetDirectoryName(coreLib)!, coreDll)))
            references.Add(MetadataReference.CreateFromFile(dllPath));
        return references;
    }

    [GeneratedRegex(@"(?<!\.)\./")]
    private static partial Regex PathSepRegex();
}