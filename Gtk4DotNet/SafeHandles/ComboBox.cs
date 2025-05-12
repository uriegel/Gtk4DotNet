namespace GtkDotNet.SafeHandles;

public class ComboBoxHandle : WidgetHandle
{
    public ComboBoxHandle() : base() { }
    public ComboBoxHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ComboBoxHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ComboBoxHandleExtensions
{
    public static ComboBoxHandle DownCastComboBox(this WidgetHandle widget) => new(widget);
}
