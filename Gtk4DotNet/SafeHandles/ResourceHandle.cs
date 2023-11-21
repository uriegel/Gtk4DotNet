using LinqTools;

namespace GtkDotNet.SafeHandles;

public class ResourceHandle : GtkHandle
{
    public ResourceHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => Resource.Unref(handle));
}
