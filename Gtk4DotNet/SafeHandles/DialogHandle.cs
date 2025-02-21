namespace GtkDotNet.SafeHandles;

public class DialogHandle : WindowHandle
{
    public DialogHandle() : base() { }
    public DialogHandle(nint obj) : base() => SetInternalHandle(obj);
}

