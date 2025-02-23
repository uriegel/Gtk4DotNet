namespace GtkDotNet.SafeHandles;

public class ComboBoxHandle : WidgetHandle
{
    public ComboBoxHandle() : base() { }
    public ComboBoxHandle(nint obj) : base() => SetInternalHandle(obj);
}
