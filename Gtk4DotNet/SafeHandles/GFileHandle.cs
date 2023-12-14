using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class GFileHandle : ObjectHandle
{
    public GFileHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Free(handle));
}
