namespace GtkDotNet.SafeHandles;

public class SignalListItemFactoryHandle : ListItemFactoryHandle
{
    public SignalListItemFactoryHandle() : base() { }
    public SignalListItemFactoryHandle(nint obj) : base() => SetInternalHandle(obj);
}

