namespace GtkDotNet.SafeHandles;

public class ComboBoxTextHandle : ComboBoxHandle
{
    public ComboBoxTextHandle() : base() { }
    public ComboBoxTextHandle(nint obj) : base() => SetInternalHandle(obj);
}
