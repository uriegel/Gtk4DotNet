namespace GtkDotNet.SafeHandles;

public class EntryHandle : WidgetHandle
{
    public EntryHandle() : base() { }
    public EntryHandle(nint obj) : base() => SetInternalHandle(obj);

    internal EntryHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class EntryHandleExtensions
{
    public static EntryHandle DownCastEntry(this WidgetHandle widget) => new(widget);
}





