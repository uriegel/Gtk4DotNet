using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class SurfaceHandle : ObjectHandle
{
    public SurfaceHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => Cairo.SurfaceDestroy(handle));
}
