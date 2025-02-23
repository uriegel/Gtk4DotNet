namespace GtkDotNet.SafeHandles;

public class MenuItemHandle : WidgetHandle
{
    public MenuItemHandle() : base() { }
    public MenuItemHandle(nint obj) : base() => SetInternalHandle(obj);
}

