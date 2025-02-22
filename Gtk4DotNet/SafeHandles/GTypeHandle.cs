namespace GtkDotNet.SafeHandles;

public class GTypeHandle : ObjectFloatingHandle
{
    public GTypeHandle() : base() { }
    public GTypeHandle(nint handle) : base() => this.handle = handle;
}

