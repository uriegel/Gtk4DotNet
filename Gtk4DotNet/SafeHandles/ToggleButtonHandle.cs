namespace GtkDotNet.SafeHandles;

public class ToggleButtonHandle : ButtonHandle
{
    public ToggleButtonHandle() : base() { }
    public ToggleButtonHandle(nint obj) : base() => SetInternalHandle(obj);
}
