using LinqTools;

namespace GtkDotNet.SafeHandles;

public abstract class ObjectHandle : BaseHandle
{
    public ObjectHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Unref(handle));
}