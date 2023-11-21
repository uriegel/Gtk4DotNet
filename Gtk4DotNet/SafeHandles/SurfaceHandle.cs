using LinqTools;

namespace GtkDotNet.SafeHandles;

public class SurfaceHandle : GtkHandle
{
    public SurfaceHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => Cairo.SurfaceDestroy(handle));
}
