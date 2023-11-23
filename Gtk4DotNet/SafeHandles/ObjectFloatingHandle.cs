using LinqTools;

namespace GtkDotNet.SafeHandles;

public abstract class ObjectFloatingHandle : ObjectHandle
{
    public ObjectFloatingHandle() : base() {}

    internal void RefSink() => floating = false;

    protected override bool ReleaseHandle() => 
        floating 
        || true.SideEffect(_ => GObject.Unref(handle));

    bool floating = true;
}