using CSScratch.Library;

namespace CSScratch.AST;

public class Function : Statement
{
    public Function(
        Name name,
        bool isLocal,
        ParameterList parameterList,
        TypeRef? returnType = null,
        Block? body = null,
        List<AttributeList>? attributeLists = null,
        bool isPublic = false
    )
    {
        Name = name;
        IsLocal = isLocal;
        ParameterList = parameterList;
        Body = body;
        ReturnType = returnType;
        AttributeLists = attributeLists ?? [];
        AddChild(Name);
        AddChild(ParameterList);
        if (ReturnType != null) AddChild(ReturnType);
        if (Body != null) AddChild(Body);
        AddChildren(AttributeLists);
        IsPublic = isPublic;
    }

    public Name Name { get; }
    public bool IsLocal { get; }
    public bool IsPublic { get; }
    public ParameterList ParameterList { get; }
    public Block? Body { get; }
    public TypeRef? ReturnType { get; }
    public List<AttributeList> AttributeLists { get; }

    public override void Compile(GbWriter writer)
    {
        writer.WriteFunction(Name.ToString(), ReturnType?.Path, ParameterList.Parameters, Body!, IsPublic);
        if (AttributeLists.Count == 0)
        {
            return;
        }

        foreach (var attribute in AttributeLists[0].Attributes)
        {
            if (attribute is ScratchEventAttribute eventAttribute)
            {
                switch (eventAttribute.Type)
                {
                    case ScratchEventType.Flag:
                        writer.WriteHandler("flag", Name.ToString());
                        break;
                    case ScratchEventType.Message:
                        writer.WriteHandler($" \"{eventAttribute.Extra}\"", Name.ToString());
                        break;
                    case ScratchEventType.Key:
                        writer.WriteHandler($"key \"{eventAttribute.Extra}\"", Name.ToString());
                        break;
                    case ScratchEventType.Click:
                        writer.WriteHandler("click", Name.ToString());
                        break;
                    case ScratchEventType.Backdrop:
                        writer.WriteHandler($"backdrop \"{eventAttribute.Extra}\"", Name.ToString());
                        break;
                    case ScratchEventType.Loudness:
                        writer.WriteHandler($"loudness > {eventAttribute.Extra}", Name.ToString());
                        break;
                    case ScratchEventType.Timer:
                        writer.WriteHandler($"timer > {eventAttribute.Extra}", Name.ToString());
                        break;
                    case ScratchEventType.Clone:
                        writer.WriteHandler("clone", Name.ToString());
                        break;
                    case ScratchEventType.Tick:
                        // Tick is a message that is sent every frame
                        writer.WriteHandler(" \"TICK\"", Name.ToString());
                        break;
                    case ScratchEventType.Initialize:
                        // Initialize is a message that is sent when the program starts
                        writer.WriteHandler(" \"INITIALIZE\"", Name.ToString());
                        break;
                    default:
                        throw new InvalidOperationException("Unknown ScratchEventType");
                }
            }
            else
            {
                throw new NotImplementedException($"Attribute type {attribute.GetType()} not implemented.");
            }
        }
    }
}