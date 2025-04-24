using CSScratch.Library;

namespace CSScratch.AST;

public class ScratchNoWarpAttribute : BaseAttribute
{
    public ScratchNoWarpAttribute()
    {
    }

    public override void Compile(GbWriter writer)
    {
        throw new NotImplementedException();
    }
}