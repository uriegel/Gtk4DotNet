using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class ResourceHandle : ObjectHandle
{
    public ResourceHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => Resource.Unref(handle));
}
