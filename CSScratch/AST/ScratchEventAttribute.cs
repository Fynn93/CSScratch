using CSScratch.Library;

namespace CSScratch.AST;

public class ScratchEventAttribute : BaseAttribute
{
    public ScratchEventType Type { get; }
    public dynamic? Extra { get; }

    public ScratchEventAttribute(ScratchEventType type)
    {
        Type = type;
        Extra = null;
    }

    public ScratchEventAttribute(ScratchEventType type, int extra)
    {
        Type = type;
        Extra = extra;
    }

    public ScratchEventAttribute(ScratchEventType type, string? extra)
    {
        Type = type;
        Extra = extra;
    }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}