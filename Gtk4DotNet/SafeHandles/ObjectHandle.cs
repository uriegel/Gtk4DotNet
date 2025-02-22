using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class ObjectHandle : BaseHandle
{
    public ObjectHandle() : base() {}

    protected override bool ReleaseHandle() 
        => true.SideEffect(_ => GObject.Unref(handle));
}