namespace GtkDotNet.SafeHandles;

public class StackSwitcherHandle : BoxHandle
{
    public StackSwitcherHandle() : base() { }
    public StackSwitcherHandle(nint obj) : base() => SetInternalHandle(obj);
}
