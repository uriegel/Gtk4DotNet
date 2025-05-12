namespace GtkDotNet.SafeHandles;

public class ComboBoxTextHandle : ComboBoxHandle
{
    public ComboBoxTextHandle() : base() { }
    public ComboBoxTextHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ComboBoxTextHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ComboBoxTextHandleExtensions
{
    public static ComboBoxTextHandle DownCastComboBoxText(this WidgetHandle widget) => new(widget);
}

