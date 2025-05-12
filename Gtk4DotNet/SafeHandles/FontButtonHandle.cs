namespace GtkDotNet.SafeHandles;

public class FontButtonHandle : ButtonHandle
{
    public FontButtonHandle() : base() { }
    public FontButtonHandle(nint obj) : base() => SetInternalHandle(obj);

    internal FontButtonHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class FontButtonHandleExtensions
{
    public static FontButtonHandle DownCastFontButton(this WidgetHandle widget) => new(widget);
}



