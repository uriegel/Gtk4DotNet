using LinqTools;

namespace GtkDotNet.SafeHandles;

public class TextBufferHandle : ObjectHandle
{
    public TextBufferHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Free(handle));
}
