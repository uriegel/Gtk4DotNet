namespace GtkDotNet.SafeHandles;

public class TextViewHandle : WidgetHandle
{
    public TextViewHandle() : base() { }

    internal TextViewHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class TextViewHandleExtensions
{
    public static TextViewHandle DownCastTextViewHandle(this WidgetHandle widget) => new(widget);
}


