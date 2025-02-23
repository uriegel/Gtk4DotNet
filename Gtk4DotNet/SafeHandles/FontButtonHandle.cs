namespace GtkDotNet.SafeHandles;

public class FontButtonHandle : ButtonHandle
{
    public FontButtonHandle() : base() { }
    public FontButtonHandle(nint obj) : base() => SetInternalHandle(obj);
}

