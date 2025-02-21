namespace GtkDotNet.SafeHandles;

public class TextViewHandle : WidgetHandle
{
    public TextViewHandle() : base() { }
    public TextViewHandle(nint obj) : base() => SetInternalHandle(obj);

    internal TextViewHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class TextViewHandleExtensions
{
    public static TextViewHandle DownCastTextViewHandle(this WidgetHandle widget) => new(widget);
}


