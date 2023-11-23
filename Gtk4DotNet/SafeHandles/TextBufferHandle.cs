using LinqTools;

namespace GtkDotNet.SafeHandles;

public class TextBufferHandle : GtkHandle
{
    public TextBufferHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Free(handle));
}
