namespace GtkDotNet.SafeHandles;

public class MenuButtonHandle : ToggleButtonHandle
{
    public MenuButtonHandle() : base() { }
    public MenuButtonHandle(nint obj) : base() => SetInternalHandle(obj);
}

