namespace GtkDotNet.SafeHandles;

public class MenuButtonHandle : WidgetHandle
{
    public MenuButtonHandle() : base() { }
    public MenuButtonHandle(nint obj) : base() => SetInternalHandle(obj);
}

