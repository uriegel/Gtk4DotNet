using LinqTools;

namespace GtkDotNet.SafeHandles;

public class CairoHandle : GtkHandle
{
    public CairoHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => Cairo.Destroy(handle));
}
