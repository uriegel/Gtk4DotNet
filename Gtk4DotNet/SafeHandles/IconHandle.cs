namespace GtkDotNet.SafeHandles;

public class IconHandle : ObjectHandle
{
    public IconHandle() : base() { }
    public IconHandle(nint obj) : base() => SetInternalHandle(obj);
}
