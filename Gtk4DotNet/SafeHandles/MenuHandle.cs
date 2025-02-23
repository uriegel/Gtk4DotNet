namespace GtkDotNet.SafeHandles;

public class MenuHandle : WidgetHandle
{
    public MenuHandle() : base() { }
    public MenuHandle(nint obj) : base() => SetInternalHandle(obj);
}

