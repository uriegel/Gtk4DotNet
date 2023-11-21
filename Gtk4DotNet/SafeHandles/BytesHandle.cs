using LinqTools;

namespace GtkDotNet.SafeHandles;

public class BytesHandle : GtkHandle
{
    public BytesHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GBytes.Unref(handle));
}
