using LinqTools;

namespace GtkDotNet.SafeHandles;

public class BytesHandle : BaseHandle
{
    public BytesHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GBytes.Unref(handle));
}
