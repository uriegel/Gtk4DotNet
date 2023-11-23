using LinqTools;

namespace GtkDotNet.SafeHandles;

public class GFileHandle : GtkHandle
{
    public GFileHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Free(handle));
}
