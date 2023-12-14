using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class CairoHandle : ObjectHandle
{
    public CairoHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => Cairo.Destroy(handle));
}
