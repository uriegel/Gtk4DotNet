using LinqTools;

namespace GtkDotNet.SafeHandles;

public class GObjectRefHandle : GtkHandle
{
    public GObjectRefHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Unref(handle));
}
