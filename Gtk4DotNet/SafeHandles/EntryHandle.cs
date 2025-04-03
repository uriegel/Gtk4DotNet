namespace GtkDotNet.SafeHandles;

public class EntryHandle : WidgetHandle
{
    public EntryHandle() : base() { }
    public EntryHandle(nint obj) : base() => SetInternalHandle(obj);
}



