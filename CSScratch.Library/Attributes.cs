namespace CSScratch.Library;

[AttributeUsage(AttributeTargets.Method)]
public class ScratchEvent : Attribute
{
    public ScratchEvent(ScratchEventType type)
    {
    }

    public ScratchEvent(ScratchEventType type, int extra)
    {
    }

    public ScratchEvent(ScratchEventType type, string extra)
    {
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ScratchNoWarp : Attribute
{
    public ScratchNoWarp()
    {
    }
}